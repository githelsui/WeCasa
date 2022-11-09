using Microsoft.Data.SqlClient;
using HAGSJP.WeCasa.Models;
using System.Data;
using MySqlConnector;

namespace HAGSJP.WeCasa.sqlDataAccess
{
    /// <summary>
    /// MariaDB Server Data Access Object aka an object for 
    /// Connecting to MariaDB Server to perform database operations
    /// https://www.nuget.org/packages/MySqlConnector/
    /// </summary>
    public class MariaDbDAO
    {
        private string _connectionString;

        public MariaDbDAO() { }

        public MySqlConnectionStringBuilder BuildConnectionString()
        {
             var builder = new MySqlConnectionStringBuilder
             {
                 Server = "localhost",
                 Port = 3306,
                 UserID = "HAGSJP.WeCasa.SqlUser",
                 Password = "cecs491",
                 Database = "HAGSJP.WeCasa"
             };

            return builder;
        }

        /// <summary>
        /// Based on the number of rows passed in, returns a result
        /// that is readable by source call
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        public Result ValidateSqlStatement(int rows)
        {
            var result = new Result();

            if (rows == 1)
            {
                result.IsSuccessful = true;
                result.ErrorMessage = string.Empty;

                return result;
            }
            result.IsSuccessful = false;
            result.ErrorMessage = $"Rows affected were not 1. It was {rows}";

            return result;
        }

        /// <summary>
        /// Synchronous method for connecting to Users database for 
        /// persisting a new user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="username"></param>
        /// <param name="otp_phrase"></param>
        /// <returns></returns>
        public Result AddUser(string email, string username, string password)
        {
            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                // Insert SQL statement
                var insertSql = @"INSERT INTO `Users` (`username`, `email`, `password`, `is_enabled`, `is_admin`) values (@username, @email, @password, 0, 0);";

                var command = connection.CreateCommand();
                command.CommandText = insertSql;
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@password", password);

                // Execution of SQL
                var rows = command.ExecuteNonQuery();

                connection.Close();

                var result = ValidateSqlStatement(rows);
                return result;
            }
        }

        /// <summary>
        /// Asynchronous method that connects to the Logs database
        /// and persists a record
        /// </summary>
        /// <param name="message"></param>
        /// <returns>bool Result</returns>
        public async Task<Result> LogData(string message, string category, string logLevel, DateTime dateTime, int userId)
        {
            long timestamp = ((DateTimeOffset)dateTime).ToUnixTimeSeconds();

            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Insert SQL statement
                var insertSql = @"INSERT INTO `Logs` (`Message`, `Log_Level`, `Category` `Timestamp`, `User_Id`) values (@message, @logLevel, @category, @timestamp, @userId);";

                var command = connection.CreateCommand();
                command.CommandText = insertSql;
                command.Parameters.AddWithValue("@message", message);
                command.Parameters.AddWithValue("@logLevel", logLevel);
                command.Parameters.AddWithValue("@category", category);
                command.Parameters.AddWithValue("@timestamp", timestamp);
                command.Parameters.AddWithValue("@userId", userId);

                // Execution of SQL
                var rows = command.ExecuteNonQuery();

                connection.Close();

                var result = ValidateSqlStatement(rows);
                return result;
            }
        }
    }
}