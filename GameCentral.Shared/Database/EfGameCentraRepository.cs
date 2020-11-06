using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameCentral.Shared.Entities;
using GameCentral.Shared.Exceptions;

namespace GameCentral.Shared.Database {
    public class EfGameCentralRepository: IGameService {

        public GameCentralContext GameCentralContext { get; }

        public EfGameCentralRepository(GameCentralContext gameCentralContext) {
            GameCentralContext = gameCentralContext;
        }

        public virtual void Dispose() {
        }

        public virtual async Task RemoveGameAsync(int id) {
            var game = GameCentralContext.Games.FirstOrDefault(g => g.GameId == id);
            if (game == null) {
                return;
            }
            GameCentralContext.Games.Remove(game);
            await GameCentralContext.SaveChangesAsync();
        }

        public  virtual  async Task AddGameAsync(Game game) {
            game.GameId = 100;
            await GameCentralContext.Games.AddAsync(game);
            await GameCentralContext.SaveChangesAsync();
        }

        public virtual  async Task EditGameAsync(Game game) {

            var g =
                from game1 in GameCentralContext.Games
                where game1.GameId == game.GameId
                select game1;
            
            if (g.Count() > 1) {
                throw new ManyGamesFoundException();
            }

            if (!g.Any()) {
                throw new GameNotExistsException();
            }

            var a = g.First();
            a.Cost = game.Cost;
            a.Description = game.Description;
            a.Genre = game.Genre;
            a.Studio = game.Studio;
            a.Publisher = game.Publisher;
            a.PreviewImageUrl = game.PreviewImageUrl;
            a.Title = game.Title;

            await GameCentralContext.SaveChangesAsync();
        }

        public virtual  async Task<Game> GetGameAsync(int id) {
            return await Task.Run(() => {
                var game =
                    from game1 in GameCentralContext.Games
                    where game1.GameId == id
                    select game1;

                if (game.Count() > 1) {
                    throw new ManyGamesFoundException();
                }

                if (!game.Any()) {
                    throw new GameNotExistsException();
                }

                return game.First();
            });
        }

        public virtual  async Task<IEnumerable<Game>> GetGamesAsync() {
            return await Task.Run(() => GameCentralContext.Games);
        }
    }
}