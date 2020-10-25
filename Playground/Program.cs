using System;
using GameCentral.Shared.Database;
using GameCentral.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Playground {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Hello World!");
            var game = new Game {
                Cost = 30,
                Description = "ddddd",
                Genre = "ddddd",
                PreviewImageUrl = "uuuu",
                Publisher = "hhhhh",
                Studio = "jjjjjj",
                Title = "dg"
            };
            using var context = new GameCentralContext(new DbContextOptions<GameCentralContext> {
                
            });
            using var repo = new EfGameCentralRepository(context);
        }
    }
}