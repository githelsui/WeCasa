using System;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Models.Security;
using HAGSJP.WeCasa.sqlDataAccess.Abstractions;
using MySqlConnector;

namespace HAGSJP.WeCasa.sqlDataAccess
{
	public class HashDAO : IHashDAO
	{
        private string _connectionString;

        public HashDAO() {}

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

        public AuthResult ValidateSqlStatement(int rows)
        {
            var result = new AuthResult();

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

        public AuthResult GetEncryptedPassword(string username)
        {
            var result = new AuthResult();

            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                // Select SQL statement
                var selectSql = @"SELECT `password` FROM `Users` WHERE `username` = @username;";

                var command = connection.CreateCommand();
                command.CommandText = selectSql;
                command.Parameters.AddWithValue("@username".ToLower(), username.ToLower());

                // Execution of SQL
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.IsSuccessful = true;
                    result.Message = string.Empty;
                    result.ReturnedObject = reader.GetString(0);
                    return result;
                }
                connection.Close();

                // Failure Cases
                result.IsSuccessful = false;
                result.Message = "Error fetching encrypted password from database.";
                return result;
            }
        }

        public AuthResult GetSalt(string username)
        {
            var result = new AuthResult();

            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                // Select SQL statement
                var selectSql = @"SELECT `salt` FROM `Users` WHERE `username` = @username;";

                var command = connection.CreateCommand();
                command.CommandText = selectSql;
                command.Parameters.AddWithValue("@username".ToLower(), username.ToLower());

                // Execution of SQL
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.IsSuccessful = true;
                    result.Message = string.Empty;

                    // User does not have a salt initizlied yet
                    if (reader.IsDBNull(0))
                    {
                        result.ReturnedObject = "";

                    }
                    else
                    {
                        result.ReturnedObject = reader.GetString(0);
                    }
                    return result;
                }
                connection.Close();

                // Failure Cases
                result.IsSuccessful = false;
                result.Message = "Error fetching salt from database.";
                return result;
            }
        }
    }
}

