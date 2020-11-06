using System;
using Confluent.Kafka;
using System.Text.Json;
using GameCentral.Shared.Entities;

namespace GameCentral.Shared.Database {
    public class GameSerializer : ISerializer<Game> {
        public byte[] Serialize(Game data, SerializationContext context)
            => JsonSerializer.SerializeToUtf8Bytes(data);
    }

    public class GameDeserializer : IDeserializer<Game> {
        public Game Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context) =>
            JsonSerializer.Deserialize<Game>(data);
    }
}