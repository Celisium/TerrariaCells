using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Terraria.ModLoader;

namespace TerrariaCells.WorldGen {

	public class NPCTypeConverter : JsonConverter<int> {
		public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
			switch (reader.TokenType) {
				case JsonTokenType.String:
					var found = ModContent.TryFind(reader.GetString(), out ModNPC modNPC);
					if (found) {
						return modNPC.Type;
					} else {
						throw new JsonException("Invalid NPC ID");
					}
				case JsonTokenType.Number:
					return reader.GetInt32();
				default:
					throw new JsonException("Invalid NPC ID");
			};
		}

		public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options) {
			var type = ModContent.GetModNPC(value);
			writer.WriteStringValue(type.FullName);
		}
	}

	public class RoomConfigSpawn {
		[JsonConverter(typeof(NPCTypeConverter))]
		[JsonRequired]
		public int NPC { get; set; }
		[JsonRequired]
		public int X { get; set; }
		[JsonRequired]
		public int Y { get; set; }
	}

	public class RoomConfig {
		public bool Surface { get; set; }
		public List<RoomConfigSpawn> Spawns { get; set; }
	}
}
