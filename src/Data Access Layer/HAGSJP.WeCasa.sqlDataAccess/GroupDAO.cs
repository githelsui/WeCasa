using HAGSJP.WeCasa.Models;
using MySqlConnector;
using HAGSJP.WeCasa.sqlDataAccess.Abstractions;
using HAGSJP.WeCasa.Models.Security;
using System.Text.Json;
using Azure;
using System.Data;


namespace HAGSJP.WeCasa.sqlDataAccess
{
   /// <summary>
   /// MariaDB Server Data Access Object aka an object for
   /// Connecting to MariaDB Server to perform database operations
   /// https://www.nuget.org/packages/MySqlConnector/
   /// </summary>
   public class GroupDAO : AccountMariaDAO
   {
       private string _connectionString;

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

        public List<string> GetMembersUsername(string groupId)
        {
           _connectionString = BuildConnectionString().ConnectionString;
            using(var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                List<string> usernames = new List<string>();

                var insertSql = @"SELECT * from USERS 
                                        WHERE group_id = @groupId;";
                var command = connection.CreateCommand();
                command.CommandText = insertSql;
                command.Parameters.AddWithValue("@groupId".ToLower(), groupId.ToLower());

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string username = reader.GetString(reader.GetOrdinal("username"));
                        usernames.Add(username);
                    }
                }
                return usernames;
            }
        }
        
   }
}