using System.Collections.Generic;
using System.Threading.Tasks;
using GameCentral.Shared.Entities;
using GameCentral.Shared.Exceptions;

namespace GameCentral.Shared.Database {
    public class FakeDatabase : IDatabase {
        private readonly Dictionary<int, Game> _games = new Dictionary<int, Game>() {
            {0, new Game() {GameId = 0, Cost = 10, Description = "disc", Genre = "RPG", Publisher = "EA"}},
            {1, new Game() {GameId = 1, Cost = 10, Description = "disc", Genre = "RPG", Publisher = "EA"}},
            {2, new Game() {GameId = 2, Cost = 10, Description = "disc", Genre = "RPG", Publisher = "EA"}},
            {3, new Game() {GameId = 3, Cost = 10, Description = "disc", Genre = "RPG", Publisher = "EA"}},
            {4, new Game() {GameId = 4, Cost = 10, Description = "disc", Genre = "RPG", Publisher = "EA"}}
        };

        public IEnumerable<Game> Games => _games.Values;

        public async Task RemoveGameAsync(int id) {
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

        public async Task<Game> GetGameAsync(int id) {
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