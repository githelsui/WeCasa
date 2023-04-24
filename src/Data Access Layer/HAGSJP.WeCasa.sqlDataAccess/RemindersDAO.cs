using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.RegularExpressions;
using HAGSJP.WeCasa.Models;
using MySqlConnector;

namespace HAGSJP.WeCasa.sqlDataAccess
{
    public class RemindersDAO : AccountMariaDAO
    {
        private string _connectionString;
        private DAOResult result;

        public RemindersDAO()
        {
            _connectionString = BuildConnectionString().ConnectionString;
        }

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

        public GroupResult GetGroupEmail(GroupModel groupModel)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var result = new GroupResult();
                // SQL statement
                var selectSql = @"SELECT username
                                  FROM UserGroups
                                  WHERE group_id = @group_id";

                var command = connection.CreateCommand();
                command.CommandText = selectSql;
                command.Parameters.AddWithValue("@group_id", groupModel.GroupId);

                // Execution of SQL
                var groupMembers = new List<string>();

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    groupMembers.Add(reader.GetString(reader.GetOrdinal("username")));
                }

                result.ReturnedObject = groupMembers;
                
                result.IsSuccessful = true;
                connection.Close();

                return result;
            }
        }
    }
}
