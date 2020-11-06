using System;
using System.Threading;
using GameCentral.Shared.Database;
using Microsoft.EntityFrameworkCore;

namespace GameCentral.DatabaseService {
    class Program {
        static void Main(string[] args) {
            if (args.Length == 0 || !int.TryParse(args[0], out _)) return;
            
            Console.WriteLine("GameCentral Database Kafka Service");
            Console.WriteLine("Port: " + args[0]);
            Console.WriteLine("Ctrl + C to exit");
            Console.WriteLine("=====================================");
            
            var source = new CancellationTokenSource();
            var token = source.Token;
            Console.CancelKeyPress += delegate { source.Cancel(); };

            var options = new DbContextOptionsBuilder<GameCentralContext>();
            options.UseSqlServer(@"Server=localhost\SQLEXPRESS;Database=GameCentral;Trusted_Connection=True;Integrated Security=True");
            using var context = new GameCentralContext(options.Options);
            var repository = new EfGameCentralRepository(context);
            
            
            
            
        }
    }
}