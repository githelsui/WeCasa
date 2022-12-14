using System;
using Microsoft.Data.SqlClient;
using HAGSJP.WeCasa.Models;
using System.Data;
using MySqlConnector;
using HAGSJP.WeCasa.sqlDataAccess.Abstractions;
using HAGSJP.WeCasa.Models.Security;
using System.Text.Json;
using System.Net;

namespace HAGSJP.WeCasa.sqlDataAccess
{
    public class AuthorizationDAO : IAuthorizationDAO
    {
        private string _connectionString;

        public AuthorizationDAO() {}

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

        public AuthResult GetRole(UserAccount ua)
        {
            var result = new AuthResult();

            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                // Select SQL statement
                var selectSql = @"SELECT `is_admin` FROM `Users` WHERE `username` = @username;";

                var command = connection.CreateCommand();
                command.CommandText = selectSql;
                command.Parameters.AddWithValue("@username".ToLower(), ua.Username.ToLower());

                // Execution of SQL
                var reader = command.ExecuteReader();

                while(reader.Read())
                {
                    result.IsSuccessful = true;
                    result.Message = string.Empty;
                    int is_admin = reader.GetByte(0);
                    switch (is_admin)
                    {
                        case 0:
                            result.ReturnedObject = UserRoles.GenericUser;
                            break;
                        case 1:
                            result.ReturnedObject = UserRoles.AdminUser;
                            break;
                    }
                    return result;
                }
                connection.Close();

                // Failure Cases
                result.IsSuccessful = false;
                result.Message = "Error fetching role from database.";
                return result;
            }
        }

        public AuthResult GetClaims(UserAccount ua)
        {
            var result = new AuthResult();

            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                // Select SQL statement
                var selectSql = @"SELECT `claims` FROM `Users` WHERE `username` = @username;";

                var command = connection.CreateCommand();
                command.CommandText = selectSql;
                command.Parameters.AddWithValue("@username".ToLower(), ua.Username.ToLower());

                // Execution of SQL
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.IsSuccessful = true;
                    result.Message = string.Empty;

                    // User does not have any claims initizlied yet
                    if(reader.IsDBNull(0))
                    {
                        Claims newClaims = new Claims();
                        newClaims.UserClaims = new List<Claim>();
                        result.ReturnedObject = newClaims;

                    } else
                    {
                        string jsonClaims = reader.GetString(0);
                        result.ReturnedObject = new Claims(jsonClaims);
                    }
                    return result;
                }
                connection.Close();

                // Failure Cases
                result.IsSuccessful = false;
                result.Message = "Error fetching claims from database.";
                return result;
            }
        }

        // Replaces entire claims field
        public AuthResult InsertClaims(UserAccount ua, List<Claim> newClaims)
        {
            var result = new AuthResult();

            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                // Update SQL statement
                var insertSql = @"UPDATE `Users` SET `claims` = @claims WHERE `username` = @username;";

                var command = connection.CreateCommand();
                command.CommandText = insertSql;
                command.Parameters.AddWithValue("@username".ToLower(), ua.Username.ToLower());
                string claimsJSON = JsonSerializer.Serialize(newClaims);
                command.Parameters.AddWithValue("@claims", claimsJSON);

                // Execution of SQL
                var rows = (command.ExecuteNonQuery());
                connection.Close();
                Result sqlRes = ValidateSqlStatement(rows);
                result.IsSuccessful = sqlRes.IsSuccessful;
                result.Message = "Successfully updated claims for user.";
                return result;
            }
        }

        // Whether user is logged in / enabled
        public AuthResult GetActiveStatus(UserAccount ua)
        {
            var result = new AuthResult();

            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                // Select SQL statement
                var selectSql = @"SELECT `is_auth` FROM `Users` WHERE `username` = @username;";

                var command = connection.CreateCommand();
                command.CommandText = selectSql;
                command.Parameters.AddWithValue("@username".ToLower(), ua.Username.ToLower());

                // Execution of SQL
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.IsSuccessful = true;
                    result.Message = string.Empty;
                    int is_auth = reader.GetByte(0);
                    if(is_auth == 1)
                    {
                        result.ReturnedObject = true;
                    } else {
                        result.ReturnedObject = false;
                    }
                    return result;
                }
                connection.Close();

                // Failure Cases
                result.IsSuccessful = false;
                result.Message = "Error fetching active status from database.";
                return result;
            }
         }
    }
}

