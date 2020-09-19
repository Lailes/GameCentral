using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameCentral.Shared.Entities;

namespace GameCentral.Shared.Database {

    public interface IDatabase: IDisposable {
        Task RemoveGameAsync(string id);
        Task AddGameAsync(Game game);
        Task EditGameAsync(Game game);
        Task<Game> GetGameAsync(string id);
        Task<IEnumerable<Game>> GetGamesAsync();
    }
} 