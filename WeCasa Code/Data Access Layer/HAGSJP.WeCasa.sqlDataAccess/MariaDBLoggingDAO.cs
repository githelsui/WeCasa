using Microsoft.Data.SqlClient;
using HAGSJP.WeCasa.Models;
using System.Data;
using MySqlConnector;

namespace HAGSJP.WeCasa.sqlDataAccess
{
    /// <summary>
    /// SQL Server Data Access Object aka an object for 
    /// Connecting to MS SQL Server to perform database operations
    /// https://www.nuget.org/packages/MySqlConnector/
    /// </summary>
    public class MariaDBLoggingDAO
    {
        private string _connectionString; // Backing field

        public MariaDBLoggingDAO() // Default constructor
        { 
            _connectionString = @"Server=.\;Database=HAGSJP.WeCasa;Integrated Security=True;TrustServerCertificate=True";
        }

        public MariaDBLoggingDAO(string connectionString)
        {
            // Storing in private string is not 100% secure, use SecureString()
            // or encryption/decryption
            _connectionString = connectionString;
        }

        public async Task<Result> LogData(string message)
        {
            var result = new Result();

            var builder = new MySqlConnectionStringBuilder
            {

            };

            using var connection = new MySqlConnection(builder.ConnectionString);
            await connection.OpenAsync();

            // Insert SQL statement
            var insertSql = "INSERT INTO Logs (Message) values(@message)";
            var command = connection.CreateCommand();
            command.CommandText = insertSql;
            command.Parameters.AddWithValue("@message", SqlDbType.VarChar).Value = message;

            // Execution of SQL
            using var reader = await command.ExecuteReaderAsync();

            // Reading the results
            while (reader.Read())
            {
                
            }
        
            connection.Close();

            return result;
        }
    }
}