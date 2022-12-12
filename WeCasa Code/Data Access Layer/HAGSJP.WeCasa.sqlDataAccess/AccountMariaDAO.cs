using Microsoft.Data.SqlClient;
using HAGSJP.WeCasa.Models;
using System.Data;
using MySqlConnector;
using HAGSJP.WeCasa.sqlDataAccess.Abstractions;
using HAGSJP.WeCasa.Models.Security;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace HAGSJP.WeCasa.sqlDataAccess
{
    /// <summary>
    /// MariaDB Server Data Access Object aka an object for 
    /// Connecting to MariaDB Server to perform database operations
    /// https://www.nuget.org/packages/MySqlConnector/
    /// </summary>
    public class AccountMariaDAO : ILoggerDAO
    {
        private string _connectionString;

        public AccountMariaDAO() { }

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

        public Result PersistUser(UserAccount userAccount, string password)
        {
            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var result = new Result();

                // Insert SQL statement
                var insertSql = @"INSERT INTO `Users` (`username`, `password`, `is_enabled`, `is_admin`, `claims`) values (@username, @password, 0, 0, @claims);";

                var command = connection.CreateCommand();
                command.CommandText = insertSql;
                command.Parameters.AddWithValue("@username", userAccount.Username);
                command.Parameters.AddWithValue("@password", password);

                // Initial claims when new user is first registered
                List<Claim> initialClaims = new List<Claim>();
                initialClaims.Add(new Claim("Functionality", "Edit event"));
                initialClaims.Add(new Claim("Read", "Read files"));
                initialClaims.Add(new Claim("Write", "Edit note"));
                initialClaims.Add(new Claim("View", "View section"));
                string claimsJSON = JsonSerializer.Serialize(initialClaims);
                command.Parameters.AddWithValue("@claims", claimsJSON);

                // Execution of SQL
                var rows = (command.ExecuteNonQuery());
                result = ValidateSqlStatement(rows);
                return result;
            }
        }

        public Result AuthenticateUser(UserAccount userAccount, string password, OTP otp)
        {
            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var result = new Result();

                // Insert SQL statement
                var insertSql = @"SELECT FROM `Users` WHERE `username` = @username, `password = @password, `otp_code` = @otp, `is_auth` = 0;";

                var command = connection.CreateCommand();
                command.CommandText = insertSql;
                command.Parameters.AddWithValue("@username", userAccount.UserAccountId);
                command.Parameters.AddWithValue("@password", password);
                command.Parameters.AddWithValue("@otp_code", otp.Code);

                // Execution of SQL
                var rows = (command.ExecuteNonQuery());
                result = ValidateSqlStatement(rows);
                return result;
            }
        }

        public Result SaveCode(UserAccount userAccount, OTP otp)
        {
            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var result = new Result();

                // Insert SQL statement
                var insertSql = @"UPDATE `Users` SET `otp_code` = @otp_code, `otp_time` = @otp_time WHERE `id`= @userAccId";

                var command = connection.CreateCommand();
                command.CommandText = insertSql;
                command.Parameters.AddWithValue("@otp_code", otp.Code);
                command.Parameters.AddWithValue("@otp_time", otp.CreateTime);
                command.Parameters.AddWithValue("@userAccId", userAccount.UserAccountId);

                // Execution of SQL
                var rows = (command.ExecuteNonQuery());
                result = ValidateSqlStatement(rows);
                return result;
            }
        }

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