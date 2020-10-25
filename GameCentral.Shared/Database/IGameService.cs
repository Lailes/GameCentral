﻿﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameCentral.Shared.Entities;

namespace GameCentral.Shared.Database {

    public interface IGameService: IDisposable {
        Task RemoveGameAsync(int id);
        Task AddGameAsync(Game game);
        Task EditGameAsync(Game game);
        Task<Game> GetGameAsync(int id);
        Task<IEnumerable<Game>> GetGamesAsync();
    }
} 