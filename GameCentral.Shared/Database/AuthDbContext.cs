using GameCentral.Shared.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GameCentral.Shared.Database {
    public class AuthDbContext: IdentityDbContext<ApiUser> {
        
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) {
        }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);
        }
    }
}