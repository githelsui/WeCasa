﻿using HAGSJP.WeCasa.Models;
using MySqlConnector;
using HAGSJP.WeCasa.sqlDataAccess.Abstractions;
using HAGSJP.WeCasa.Models.Security;
using System.Text.Json;
using Azure;


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
                var insertSql = @"INSERT INTO `Users` (`username`, `password`, `is_enabled`, `is_admin`, `claims`) values (@username, @password, 1, 0, @claims);";

                var command = connection.CreateCommand();
                command.CommandText = insertSql;
                command.Parameters.AddWithValue("@username".ToLower(), userAccount.Username.ToLower());
                command.Parameters.AddWithValue("@password", password);

                // Initial claims when new user is first registered
                List<Claim> initialClaims = new List<Claim>
                {
                    new Claim("Functionality", "Edit event"),
                    new Claim("Read", "Read files"),
                    new Claim("Write", "Edit note"),
                    new Claim("View", "View section")
                };
                string claimsJSON = JsonSerializer.Serialize(initialClaims);
                command.Parameters.AddWithValue("@claims", claimsJSON);

                // Execution of SQL
                var rows = (command.ExecuteNonQuery());
                result = ValidateSqlStatement(rows);
                return result;
            }
        }

        // Checks if authentication pre-conditions are met
        public AuthResult GetUserInfo(UserAccount userAccount)
        {
            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var result = new AuthResult();

                // Select SQL statement
                var selectSql = @"SELECT  * 
                                    FROM  `Users` 
                                    WHERE `username` = @username";

                var command = connection.CreateCommand();
                command.CommandText = selectSql;
                command.Parameters.AddWithValue("@username".ToLower(), userAccount.Username.ToLower());

                // Execution of SQL
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result.ExistingAcc = true;
                        result.IsAuth = reader.GetInt32(reader.GetOrdinal("is_auth")) == 1 ? true : false;
                        result.IsEnabled = reader.GetInt32(reader.GetOrdinal("is_enabled")) == 1 ? true : false;
                        result.HasValidCredentials = reader.GetString(reader.GetOrdinal("password")) == userAccount.Password ? true : false;
                    }
                    // User not found
                    else
                    {
                        result.ExistingAcc = false;
                    }
                }
                return result;
            }
        }

        public AuthResult AuthenticateUser(UserAccount userAccount, OTP otp)
        {
            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var result = new AuthResult();

                // Select SQL statement
                var selectSql = @"SELECT * 
                                  FROM `Users` 
                                  WHERE `username` = @username 
                                  AND   `password` = @password 
                                  AND   `otp_code` = @otp;";

                var command = connection.CreateCommand();
                command.CommandText = selectSql;
                command.Parameters.AddWithValue("@username".ToLower(), userAccount.Username.ToLower());
                command.Parameters.AddWithValue("@password", userAccount.Password);
                command.Parameters.AddWithValue("@otp", otp.Code);

                // Execution of SQL
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Ensuring that the OTP has not expired
                        var otp_time = reader.GetDateTime(reader.GetOrdinal("otp_time"));
                        if (DateTime.Now < otp_time.AddMinutes(2))
                        {
                            result.IsSuccessful = true;
                            result.HasValidOTP = true;
                        }
                        else
                        {
                            result.ExpiredOTP = true;
                        }
                    }
                    // One-time code is incorrect
                    else
                    {
                        result.HasValidOTP = false;
                    }
                }
                return result;
            }
        }

        // Returns a list of DateTimes of all failure attempts
        public List<DateTime> GetUserOperations(UserAccount userAccount, UserOperation operation)
        {
            List<DateTime> auth_attempts = new List<DateTime>();
            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var result = new Result();

                // Select SQL statement
                var selectSql = @"SELECT Timestamp FROM `Logs` 
                                    WHERE `Username` = @username 
                                    AND `Operation` = @operation 
                                    AND Timestamp >= NOW() - INTERVAL 1 DAY;";

                var command = connection.CreateCommand();
                command.CommandText = selectSql;
                command.Parameters.AddWithValue("@username".ToLower(), userAccount.Username.ToLower());
                command.Parameters.AddWithValue("@operation", operation.ToString());

                // Execution of SQL
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    auth_attempts.Add(JsonSerializer.Deserialize<DateTime>(reader.GetString(0)));
                }
                return auth_attempts;
            }
        }
        // Resets OTP after successful use
        public Result ResetOTP(UserAccount userAccount)
        {
            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var result = new Result();

                // Update SQL statement
                var updateSql = @"UPDATE `Users` 
                                    SET `otp_code`   = NULL, 
                                        `otp_time`   = NULL,
                                        `is_auth`    = 1
                                    WHERE `username` = @username;";

                var command = connection.CreateCommand();
                command.CommandText = updateSql;
                command.Parameters.AddWithValue("@username".ToLower(), userAccount.Username.ToLower());

                // Execution of SQL
                var rows = (command.ExecuteNonQuery());
                result = ValidateSqlStatement(rows);
                connection.Close();
                return result;
            }
        }

        // Clears all failure attempts
        public Result ResetAuthenticationAttempts(UserAccount userAccount, UserOperation operation)
        {
            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var result = new Result();

                // Update SQL statement
                var updateSql = @"UPDATE `Logs` 
                                    SET `Operation`  = NULL
                                    WHERE `Username` = @username 
                                    AND `Operation`  = @operation 
                                    AND `Timestamp`  >= NOW() - INTERVAL 1 DAY;";

                var command = connection.CreateCommand();
                command.CommandText = updateSql;
                command.Parameters.AddWithValue("@username".ToLower(), userAccount.Username.ToLower());
                command.Parameters.AddWithValue("@operation", operation.ToString());

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

                // Update SQL statement
                var updateSql = @"UPDATE `Users` 
                                    SET `otp_code`  = @otp_code, 
                                        `otp_time`  = @otp_time 
                                    WHERE `username`= @username;";

                var command = connection.CreateCommand();
                command.CommandText = updateSql;
                command.Parameters.AddWithValue("@otp_code", otp.Code);
                command.Parameters.AddWithValue("@otp_time", otp.CreateTime);
                command.Parameters.AddWithValue("@username".ToLower(), userAccount.Username.ToLower());

                // Execution of SQL
                var rows = (command.ExecuteNonQuery());
                result = ValidateSqlStatement(rows);
                return result;
            }
        }

        public Result SetUserAbility(UserAccount userAccount, int isEnabled)
        {
            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var result = new Result();

                // Update SQL statement
                var updateSql = @"UPDATE `Users` 
                                    SET `is_enabled` = @is_enabled 
                                    WHERE `username` = @username";

                var command = connection.CreateCommand();
                command.CommandText = updateSql;
                command.Parameters.AddWithValue("@is_enabled", isEnabled);
                command.Parameters.AddWithValue("@username".ToLower(), userAccount.Username.ToLower());

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
                var insertSql = @"INSERT INTO `Logs` 
                                    (`Message`, `Log_Level`, `Category`, `Username`, `Operation`) 
                                VALUES 
                                    (@message, @logLevel, @category, @username, @operation);";

                var command = connection.CreateCommand();
                command.CommandText = insertSql;
                command.Parameters.AddWithValue("@message", log.Message);
                command.Parameters.AddWithValue("@logLevel", log.LogLevel);
                command.Parameters.AddWithValue("@category", log.Category);
                command.Parameters.AddWithValue("@username", log.Username);
                command.Parameters.AddWithValue("@operation", log.Operation);

                // Execution of SQL
                int rows = await command.ExecuteNonQueryAsync();

                connection.Close();

                var result = ValidateSqlStatement(rows);

                return result;
            }
        }
    }
}