﻿using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using GameCentral.Shared.Entities;
using GameCentral.Shared.Exceptions;

namespace GameCentral.Shared.Database {
    public class MsSqlServerGameService: IGameService {
        private readonly SqlConnection _sqlConnection;

        public MsSqlServerGameService() {
            _sqlConnection = new SqlConnection {
                ConnectionString = @"Server=localhost\SQLEXPRESS;Database=GameCentral;Trusted_Connection=True;"
            };
            _sqlConnection.Open();
        }

        private IEnumerable<Game> Games {
            get {
                using var command = new SqlCommand {
                    CommandText = "SELECT GameId, Title, Studio, Genre, Publisher, Cost, PreviewImageUrl FROM GameCentral.Products.Games",
                    CommandType = CommandType.Text,
                    Connection = _sqlConnection
                };

                using var reader = command.ExecuteReader();
                
                var list = new List<Game>();
                while (reader.Read()) {
                    list.Add(new Game {
                        Title = reader["Title"] as string,
                        Studio = reader["Studio"] as string,
                        Genre = reader["Genre"] as string,
                        Publisher = reader["Publisher"] as string,
                        GameId = (int) reader["GameId"],
                        Cost = (int) reader["Cost"],
                        PreviewImageUrl = reader["PreviewImageUrl"] as string
                    });
                }

                return list;
            }
        }

        public async Task RemoveGameAsync(int id) {
            await using var command = new SqlCommand {
                CommandType = CommandType.Text,
                CommandText = $"DELETE FROM GameCentral.Products.Games WHERE GameId LIKE {id}",
                Connection = _sqlConnection
            };
            if ((await command.ExecuteNonQueryAsync()) == 0) {
                throw new GameNotExistsException();
            }
        }

        public async Task AddGameAsync(Game game) {
            await using var command = new SqlCommand {
                CommandType = CommandType.Text,
                CommandText = $"INSERT INTO GameCentral.Products.Games (Title, Studio, Description, Genre, Publisher, Cost, PreviewImageUrl) VALUES (N\'{game.Title}\', N\'{game.Studio}\', N\'{game.Description}\', N\'{game.Genre}\', N\'{game.Publisher}\', \'{game.Cost}\', N\'{game.PreviewImageUrl}\')",
                Connection = _sqlConnection
            };
            
            await command.ExecuteNonQueryAsync();
        }

        public async Task EditGameAsync(Game game) {
            await using var command = new SqlCommand {
                CommandType = CommandType.Text,
                Connection = _sqlConnection,
                CommandText = $"UPDATE GameCentral.Products.Games SET Title = N\'{game.Title}\', Studio = N\'{game.Studio}\', Description = N\'{game.Description}\', Genre = N\'{game.Genre}\', Publisher = N\'{game.Publisher}\', Cost = \'{game.Cost}\', PreviewImageUrl = N\'{game.PreviewImageUrl}\' WHERE GameId = {game.GameId}"
            };
            var affectesLines = await command.ExecuteNonQueryAsync();
            if (affectesLines == 0) {
                throw new GameNotExistsException();
            }
        }

        public async Task<Game> GetGameAsync(int id) {
            await using var command = new SqlCommand {
                Connection = _sqlConnection,
                CommandType = CommandType.Text,
                CommandText = $"SELECT * FROM GameCentral.Products.Games WHERE GameId LIKE {id}"
            };
            
            await using var reader = await command.ExecuteReaderAsync();
            if (!reader.HasRows) {
                throw new GameNotExistsException();
            }
            reader.Read();
            return new Game {
                Title = reader["Title"] as string,
                Studio = reader["Studio"] as string,
                Genre = reader["Genre"] as string,
                Publisher = reader["Publisher"] as string,
                GameId = (int) reader["GameId"],
                Description = reader["Description"] as string,
                Cost = (int) reader["Cost"],
                PreviewImageUrl = reader["PreviewImageUrl"] as string
            };
        }

        public async Task<IEnumerable<Game>> GetGamesAsync() => 
            await Task.Run(() => Games);
        

        public void Dispose() {
            _sqlConnection?.Close();
            _sqlConnection?.Dispose();
        }
    }
}