using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Dapper;
using ClientManagement.Models;

namespace ClientManagement.Data
{
    public class Database
    {
        private string _connectionString;

        public Database(string connectionString)
        {
            _connectionString = connectionString;
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                connection.Execute(@"
                    CREATE TABLE IF NOT EXISTS Clients (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT,
                        Age INTEGER,
                        Comment TEXT
                    );
                    CREATE TABLE IF NOT EXISTS ActionHistory (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        ClientId INTEGER,
                        Action TEXT,
                        Timestamp DATETIME
                    );
                ");
            }
        }

        public IEnumerable<Client> GetAllClients()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                return connection.Query<Client>("SELECT * FROM Clients");
            }
        }

        public Client GetClientById(int id)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<Client>("SELECT * FROM Clients WHERE Id = @Id", new { Id = id });
            }
        }

        public void AddClient(Client client)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        connection.Execute("INSERT INTO Clients (Name, Age, Comment) VALUES (@Name, @Age, @Comment)", client, transaction: transaction);
                        var clientId = connection.QuerySingle<long>("SELECT last_insert_rowid()");
                        transaction.Commit();
                        Console.WriteLine(clientId);
                        int clientIdInt = (int)clientId;
                        Console.WriteLine(clientIdInt);
                        AddActionHistory(clientIdInt, "Added");
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction on exception
                        transaction.Rollback();
                        Console.WriteLine($"Error: {ex.Message}");
                    }                    

                }
                
            }
        }

        public void UpdateClient(Client client)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Execute("UPDATE Clients SET Name = @Name, Age = @Age, Comment = @Comment WHERE Id = @Id", client);
                AddActionHistory(client.Id, "Updated");
            }
        }

        public void DeleteClient(int id)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Execute("DELETE FROM Clients WHERE Id = @Id", new { Id = id });
                AddActionHistory(id, "Deleted");
            }
        }

        public IEnumerable<ActionHistory> GetActionHistory(int clientId)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                return connection.Query<ActionHistory>("SELECT * FROM ActionHistory WHERE ClientId = @ClientId", new { ClientId = clientId });
            }
        }

        private void AddActionHistory(int clientId, string action)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Execute("INSERT INTO ActionHistory (ClientId, Action, Timestamp) VALUES (@ClientId, @Action, @Timestamp)",
                    new { ClientId = clientId, Action = action, Timestamp = DateTime.UtcNow });
            }
        }
    }
}
