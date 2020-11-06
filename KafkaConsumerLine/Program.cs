using System;
using System.Threading;
using Confluent.Kafka;

namespace KafkaConsumerLine {
    class Program {
        private const string TopicName = "GameCentralMessages";
        
        static void Main(string[] args) {
            var config = new ConsumerConfig {
                BootstrapServers = "localhost:9092",
                GroupId = "GameCons"
            };

            using var consumer1 = new ConsumerBuilder<string, string>(config).Build();
            consumer1.Subscribe(TopicName);
            Console.WriteLine("Subscribed to topic: " + TopicName);
            Console.WriteLine("Ctrl + C to exit");

            var cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;

            Console.CancelKeyPress += delegate { cancellationTokenSource.Cancel(); };

            while (!token.IsCancellationRequested) {

                var message = consumer1.Consume(token);
                Console.WriteLine($"Offset: {message.Offset}, Key: {message.Message.Key}, Value: {message.Message.Value}");
            }

        }
    }
}