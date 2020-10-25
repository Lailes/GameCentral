using GameCentral.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameCentral.Shared.Database {
    public class GameCentralContext : DbContext {

        public DbSet<Game> Games { get; set; }

        public GameCentralContext(DbContextOptions<GameCentralContext> options): base(options) {
        }
        
    }
}