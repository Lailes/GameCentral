using System.Threading.Tasks;
using Confluent.Kafka;
using GameCentral.Shared.Entities;

namespace GameCentral.Shared.Database {

    public class KafkaEFGameRepository: EfGameCentralRepository {

        private const string Topic = "gamess";
        
        public IProducer<string, Game> Producer { get; set; }
        
        public KafkaEFGameRepository(GameCentralContext gameCentralContext) : base(gameCentralContext) {
            
            var config = new ProducerConfig {
                BootstrapServers = "localhost:9092",
            };

            Producer = new ProducerBuilder<string, Game>(config).SetValueSerializer(new GameSerializer()).Build();
        }

        public override void Dispose() {
            Producer.Dispose();
        }

        public override async Task RemoveGameAsync(int id) {
            await Producer.ProduceAsync(Topic, new Message<string, Game>{Key = "DELETE", Value = new Game(){GameId = id}});
            Producer.Flush();
        }

        public override async Task AddGameAsync(Game game) {
            await Producer.ProduceAsync(Topic, new Message<string, Game>{Key = "ADD", Value = game});
            Producer.Flush();
        }

        public override async Task EditGameAsync(Game game) {
            await Producer.ProduceAsync(Topic, new Message<string, Game>{Key = "EDIT", Value = game});
            Producer.Flush();
        }
    }
}