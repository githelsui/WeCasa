using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.RegularExpressions;
using HAGSJP.WeCasa.Models;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace HAGSJP.WeCasa.sqlDataAccess
{
    public class RemindersDAO : AccountMariaDAO
    {
        private string _connectionString;
        private MariaDB _mariaDB;
        private DAOResult _result;

        public string GetConnectionString()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("config.json")
                .AddEnvironmentVariables()
                .Build();

            _mariaDB = config.GetRequiredSection("MariaDB").Get<MariaDB>();
            return _mariaDB.Local;
        }

        public GroupResult GetGroupEmail(GroupModel groupModel)
        {
            _connectionString = GetConnectionString();
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
