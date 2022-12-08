using Microsoft.Data.SqlClient;
using HAGSJP.WeCasa.Models;
using System.Data;
using MySqlConnector;
using HAGSJP.WeCasa.sqlDataAccess.Abstractions;

namespace HAGSJP.WeCasa.sqlDataAccess
{
    /// <summary>
    /// MariaDB Server Data Access Object aka an object for 
    /// Connecting to MariaDB Server to perform database operations
    /// https://www.nuget.org/packages/MySqlConnector/
    /// </summary>
    public class MariaDbDAO : ILoggerDAO
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
                result.Message = string.Empty;

                return result;
            }
            result.IsSuccessful = false;
            result.Message = $"Rows affected were not 1. It was {rows}";

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
        public Result PersistUser(UserAccount userAccount, string password)
        {
            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var result = new Result();

                // Insert SQL statement
                var insertSql = @"INSERT INTO `Users` (`email`, `password`, `is_enabled`, `is_admin`, `is_auth`) values (@email, @password, 0, 0);";

                var command = connection.CreateCommand();
                command.CommandText = insertSql;
                command.Parameters.AddWithValue("@email", userAccount.Username);
                command.Parameters.AddWithValue("@password", password);

                // Execution of SQL
                var id = (command.ExecuteScalar());

                if (id == null)
                {
                    result = ValidateSqlStatement(0);
                } else
                {
                    result.IsSuccessful = true;
                    // Adding userID to result message
                    result.Message = id.ToString();
                }
                return result;
            }
        }

        /// <summary>
        /// Asynchronous method that connects to the Logs database
        /// and persists a record
        /// </summary>
        /// <param name="message"></param>
        /// <returns>bool Result</returns>
        public async Task<Result> LogData(Log log)
        {
            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Insert SQL statement
                var insertSql = @"INSERT INTO `Logs` (`Message`, `Log_Level`, `Category`, `Username`) values (@message, @logLevel, @category, @username);";

                var command = connection.CreateCommand();
                command.CommandText = insertSql;
                command.Parameters.AddWithValue("@message", log.Message);
                command.Parameters.AddWithValue("@logLevel", log.LogLevel);
                command.Parameters.AddWithValue("@category", log.Category);
                command.Parameters.AddWithValue("@username", log.Username);

                // Execution of SQL
                int rows = await command.ExecuteNonQueryAsync();

                connection.Close();

                var result = ValidateSqlStatement(rows);

                return result;
            }
        }
    }
}