using System;
using System.Linq;
using GameCentral.Shared.Database;
using GameCentral.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Playground {
    class Program {
        static void Main(string[] args) {
            var optionsMsSql = new DbContextOptionsBuilder<GameCentralContext>();
            optionsMsSql.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=GameCentral;Trusted_Connection=True");
            
            
            var optionsSqlite = new DbContextOptionsBuilder<GameCentralContext>();
            optionsSqlite.UseSqlite(@"Filename=C:\Tools\GameCentral.db");
            
            var context1 = new GameCentralContext(optionsMsSql.Options);
            var context2 = new GameCentralContext(optionsSqlite.Options);


            foreach (var context1Game in context1.Games) {
                context1Game.GameId = null;
                context2.Games.Add(context1Game);
            }
            context2.SaveChanges();
            /*
            Console.WriteLine("===============================");
            
            foreach (var game in context2.Games) {
                Console.WriteLine(game.Title);
            }*/

        }
    }
}