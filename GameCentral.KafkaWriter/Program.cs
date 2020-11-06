using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using GameCentral.Shared.Database;
using GameCentral.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameCentral.KafkaWriter {
    class Program {
        
        private const string Topic = "gamess";
        private const string SQLiteConnection = @"Filename=C:\\Tools\\GameCentral.db";

        public IConsumer<string, Game> Consumer { get; set; }
        
        static void Main(string[] args) {
            new Program().Run().GetAwaiter().GetResult();
        }

        async Task Run() {
            Console.WriteLine("Writer service started:");
            
            var config = new ConsumerConfig {
                BootstrapServers = "localhost:9092",
                GroupId = "GameWriter"
            };

            Consumer = new ConsumerBuilder<string, Game>(config).SetValueDeserializer(new GameDeserializer()).Build();
            Consumer.Subscribe(Topic);
            
            var contextOptionsBuilder = new DbContextOptionsBuilder<GameCentralContext>();
            contextOptionsBuilder.UseSqlite(SQLiteConnection);
            
            using var repository = new EfGameCentralRepository(new GameCentralContext(contextOptionsBuilder.Options));

            var tokenSource = new CancellationTokenSource();
            Console.CancelKeyPress += delegate { tokenSource.Cancel(); };
            var token = tokenSource.Token;

            while (!tokenSource.IsCancellationRequested) {
                var consumeResult = Consumer.Consume(token);
                Console.Write($"[Consume] Offset: {consumeResult.Offset}, Action: {consumeResult.Message.Key}, Id: {(consumeResult.Message.Value.GameId != null ? consumeResult.Message.Value.GameId.ToString() : "null")}");
                Console.WriteLine($", Title: {consumeResult.Message.Value.Title ?? "null"}");
                var message = consumeResult.Message;
                Thread.Sleep(10);
                switch (message.Key) {
                    case "ADD": {
                        try {
                            await repository.AddGameAsync(message.Value);
                        }
                        catch (Exception e) {
                            Console.WriteLine(e.Message);
                        }
                        break;
                    }
                    case "EDIT": {
                        try {
                            await repository.EditGameAsync(message.Value);
                        }
                        catch (Exception e) {
                            Console.WriteLine(e.Message);
                        }
                        break;
                    }
                    case "DELETE": {
                        try {
                            await repository.RemoveGameAsync(message.Value.GameId ?? -1);
                        }
                        catch (Exception e) {
                            Console.WriteLine(e.Message);
                        }
                        break;
                    }
                    default: {
                        Thread.Sleep(10);
                        break;
                    }
                }

            }
            
            
            Consumer.Dispose();
            Console.WriteLine("Finish");
        }
        
    }
}