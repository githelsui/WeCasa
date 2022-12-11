using System;
using Microsoft.Data.SqlClient;
using HAGSJP.WeCasa.Models;
using System.Data;
using MySqlConnector;
using HAGSJP.WeCasa.sqlDataAccess.Abstractions;
using HAGSJP.WeCasa.Models.Security;
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

        public ResultObj GetRole(UserAccount ua)
        {
            var result = new ResultObj();

            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                // Select SQL statement
                var selectSql = @"SELECT is_admin FROM `Users` where username = @username;";

                var command = connection.CreateCommand();
                command.CommandText = selectSql;
                command.Parameters.AddWithValue("@username", ua.Username);

                // Execution of SQL
                var reader = command.ExecuteReader();

                while(reader.Read())
                {
                    result.IsSuccessful = true;
                    result.Message = string.Empty;
                    int is_admin = (int)reader["is_admin"];
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

        public ResultObj GetClaims(UserAccount ua)
        {
            var result = new ResultObj();

            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                // Select SQL statement
                var selectSql = @"SELECT claims FROM `Users` where username = @username;";

                var command = connection.CreateCommand();
                command.CommandText = selectSql;
                command.Parameters.AddWithValue("@username", ua.Username);

                // Execution of SQL
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.IsSuccessful = true;
                    result.Message = string.Empty;
                    string jsonClaims = reader["claims"].ToString();
                    result.ReturnedObject = new Claims(jsonClaims);
                    return result;
                }
                connection.Close();

                // Failure Cases
                result.IsSuccessful = false;
                result.Message = "Error fetching claims from database.";
                return result;
            }
        }

        // Whether user is logged in / enabled
        public ResultObj GetActiveStatus(UserAccount ua)
        {
            var result = new ResultObj();

            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                // Select SQL statement
                var selectSql = @"SELECT is_enabled FROM `Users` where username = @username;";

                var command = connection.CreateCommand();
                command.CommandText = selectSql;
                command.Parameters.AddWithValue("@username", ua.Username);

                // Execution of SQL
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.IsSuccessful = true;
                    result.Message = string.Empty;
                    int is_enabled = (int)reader["is_enabled"];
                    if(is_enabled == 1)
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

