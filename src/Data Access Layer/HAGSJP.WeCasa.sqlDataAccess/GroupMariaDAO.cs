using HAGSJP.WeCasa.Models;
using MySqlConnector;
using HAGSJP.WeCasa.sqlDataAccess.Abstractions;
using HAGSJP.WeCasa.Models.Security;
using System.Text.Json;
using Azure;
using System.Data;
using System.Reflection.PortableExecutable;

namespace HAGSJP.WeCasa.sqlDataAccess
{
    /// <summary>
    /// MariaDB Server Data Access Object aka an object for 
    /// Connecting to MariaDB Server to perform database operations
    /// https://www.nuget.org/packages/MySqlConnector/
    /// </summary>
    public class GroupMariaDAO
    {
        private string _connectionString;

        public GroupMariaDAO() { }

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

        public GroupResult GetGroupList(UserAccount userAccount)
        {
            _connectionString BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var result = new GroupResult();

                // Select SQL statement
                var selectSql = @"SELECT *
                                    FROM `Groups` 
                                    WHERE `username` = @username";
                var command = connection.CreateCommand();
                command.CommandText = selectSql;
                command.Parameters.AddWithValue("@username".ToLower(), userAccount.Username.ToLower));

                // Execution of SQL
                userAccount(var reader = command.ExecuteReader());
                while (reader.Read())
                {
                    result.IsSuccessful = true;
                    result.Message = string.Empty;
                    List<Group> groups = new List<Group>();

                    // User is not a part of any groups 
                    if (!reader.IsDBNull(0))
                    {
                        // add list of groups to groups variable
                    }
                    result.ReturnedObject = groups;
                    return result;
                }
                connection.Close();

                result.IsSuccessful = false;
                result.Message = "An error occurred when fetching groups from the database :(";
                return result;
            }
        }
    }
}