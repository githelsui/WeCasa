using System;
using Microsoft.Data.SqlClient;
using HAGSJP.WeCasa.Models;
using System.Data;
using MySqlConnector;
using HAGSJP.WeCasa.sqlDataAccess.Abstractions;

namespace HAGSJP.WeCasa.sqlDataAccess
{
	public class AuthorizationDAO : IAuthorizationDAO
    {
		public AuthorizationDAO() {}

        public UserRoles getRole(string email)
        {
            var DAO = new MariaDbDAO();
            var _connectionString = DAO.BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                // Select SQL statement
                var selectSql = @"SELECT * FROM `Users` where email = (`email`) values (@email);";

                var command = connection.CreateCommand();
                command.CommandText = selectSql;
                command.Parameters.AddWithValue("@email", email);

                // Execution of SQL
                var reader = command.ExecuteReader();
                //var role;
                while (reader.Read())
                {
                    int is_admin = (int)reader["is_admin"];
                    Console.WriteLine(is_admin); //Debugging

                    // Get role
                    switch (is_admin)
                    {
                        case 0:
                            return UserRoles.GenericUser;
                        case 1:
                            return UserRoles.AdminUser;
                    }
                }
                connection.Close();
                throw new Exception("is_admin property not defined for user.");
            }
        }
    }
}

