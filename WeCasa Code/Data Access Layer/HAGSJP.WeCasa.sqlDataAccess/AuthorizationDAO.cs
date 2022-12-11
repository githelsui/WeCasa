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

        public UserRoles GetRole(UserAccount ua)
        {
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
                    int is_admin = (int)reader["is_admin"];
                    //return is_admin
                    // Get roel
                    switch (is_admin)
                    {
                        case 0:
                            return UserRoles.GenericUser;
                        case 1:
                            return UserRoles.AdminUser;
                    }
                }
                connection.Close();
                throw new Exception("is_admin column is not defined for this user.");
            }
        }

        public Claims GetClaims(UserAccount ua)
        {
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
                    string claims = reader["claims"].ToString();
                    return new Claims(claims);
                    //return serialized claims
                }

                connection.Close();
                throw new Exception("claims column is not defined for this user.");
            }
        }
    }
}

