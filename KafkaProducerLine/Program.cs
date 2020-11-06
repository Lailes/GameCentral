using System;
using System.Reflection.Emit;
using Confluent.Kafka;
using GameCentral.Shared.Database;
using GameCentral.Shared.Entities;

namespace KafkaProducerLine {
    class Program {
        private const string TopicName = "gamess";
        
        static void Main(string[] args) {
            var config = new ProducerConfig {
                BootstrapServers = "localhost:9092"
            };

            using var producer1 = new ProducerBuilder<string, Game>(config).SetValueSerializer(new GameSerializer()).Build();
            
            Label:
            for (int i = 0; i < 100; i++) {
                producer1.Produce(TopicName, new Message<string, Game>() {
                    Key = "SKIP", 
                    Value = new Game() {
                        GameId = i, Title = $"Title{i}"
                    }
                });
                producer1.Flush();
            }
            Console.WriteLine("Type to resend 100 messages");
            Console.ReadLine();
            goto Label;
        }
    }
}