using System;
using Microsoft.Data.SqlClient;
using HAGSJP.WeCasa.Models;
using System.Data;
using MySqlConnector;
using HAGSJP.WeCasa.sqlDataAccess.Abstractions;
using HAGSJP.WeCasa.Models.Security;

namespace HAGSJP.WeCasa.sqlDataAccess
{
	public class AuthorizationDAO : IAuthorizationDAO
    {
		public AuthorizationDAO() {}

        public UserRoles GetRole(UserAccount ua)
        {
            var DAO = new AccountMariaDAO();
            var _connectionString = DAO.BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                // Select SQL statement
                var selectSql = @"SELECT * FROM `Users` where username = @username;";

                var command = connection.CreateCommand();
                command.CommandText = selectSql;
                command.Parameters.AddWithValue("@username", ua.Username);

                // Execution of SQL
                var reader = command.ExecuteReader();

                while(reader.Read())
                {
                    int is_admin = (int)reader["is_admin"];
                    
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
                throw new Exception("is_admin property is not defined for this user.");
            }
        }
    }
}

