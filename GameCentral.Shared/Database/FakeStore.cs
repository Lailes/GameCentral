using System.Collections.Generic;
using System.Threading.Tasks;
using GameCentral.Shared.Entities;
using GameCentral.Shared.Exceptions;

namespace GameCentral.Shared.Database {
    public class FakeDatabase : IDatabase {
        private readonly Dictionary<string, Game> _games = new Dictionary<string, Game>() {
            {"one", new Game() {GameId = "one", Cost = 10, Description = "disc", Genre = "RPG", Publisher = "EA"}},
            {"two", new Game() {GameId = "two", Cost = 10, Description = "disc", Genre = "RPG", Publisher = "EA"}},
            {"three", new Game() {GameId = "three", Cost = 10, Description = "disc", Genre = "RPG", Publisher = "EA"}},
            {"four", new Game() {GameId = "four", Cost = 10, Description = "disc", Genre = "RPG", Publisher = "EA"}},
            {"five", new Game() {GameId = "five", Cost = 10, Description = "disc", Genre = "RPG", Publisher = "EA"}}
        };

        public IEnumerable<Game> Games => _games.Values;

        public async Task RemoveGameAsync(string id) {
            var wasDeleted = _games.Remove(id);
            if (!wasDeleted) {
                throw new GameNotExistsException();
            }
        }

        public async Task AddGameAsync(Game game) {
            if (_games.ContainsKey(game.GameId)) {
                throw new GameExistsException();
            }

            _games.Add(game.GameId, game);
        }

        public async Task EditGameAsync(Game game) {
            if (!_games.ContainsKey(game.GameId)) {
                throw new GameNotExistsException();
            }

            _games.Add(game.GameId, game);
        }

        public async Task<Game> GetGameAsync(string id) {
            if (!_games.ContainsKey(id)) {
                throw new GameNotExistsException();
            }

            return _games[id];
        }


        public async Task<IEnumerable<Game>> GetGamesAsync() {
            return await Task.Run(() => Games);
        }

        public void Dispose() {
        }
    }
}