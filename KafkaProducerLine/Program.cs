using System;
using Confluent.Kafka;


namespace KafkaProducerLine {
    class Program {
        private const string TopicName = "GameCentralMessages";
        
        static void Main(string[] args) {
            var config = new ProducerConfig {
                BootstrapServers = "localhost:9092"
            };

            using var producer1 = new ProducerBuilder<string, string>(config).Build();
            using var producer2 = new ProducerBuilder<string, string>(config).Build();

            for (int i = 0; i < 10; i++) {
                producer1.Produce(TopicName, new Message<string, string> {
                    Value = (i).ToString(),
                    Key = (-i).ToString()
                });
            }

            producer1.Flush();
            for (int i = 100; i < 110; i++) {
                producer2.Produce(TopicName, new Message<string, string> {
                    Value = (i).ToString(),
                    Key = (-i).ToString()
                });
            }

            producer2.Flush();

        }
    }
}