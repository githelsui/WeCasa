﻿using HAGSJP.WeCasa.Models;
using MySqlConnector;
using HAGSJP.WeCasa.sqlDataAccess.Abstractions;
using HAGSJP.WeCasa.Models.Security;
using System.Text.Json;
using Azure;
using System.Data;
using System.Reflection.PortableExecutable;
using System.Data.SqlTypes;

namespace HAGSJP.WeCasa.sqlDataAccess
{
    /// <summary>
    /// MariaDB Server Data Access Object aka an object for 
    /// Connecting to MariaDB Server to perform database operations
    /// https://www.nuget.org/packages/MySqlConnector/
    /// </summary>
    public class GroupMariaDAO : ILoggerDAO
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
            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var result = new GroupResult();

                // Select SQL statement
                var selectSql = @"SELECT *
                                    FROM `Groups`
                                 INNER JOIN `UserGroups` 
                                    ON `Groups`.`group_id` = `UserGroups`.`group_id`
                                 WHERE `UserGroups`.`username` = @username";
                var command = connection.CreateCommand();
                command.CommandText = selectSql;
                command.Parameters.AddWithValue("@username".ToLower(), userAccount.Username.ToLower());

                // Execution of SQL
                var reader = command.ExecuteReader();
                List<GroupModel> groups = new List<GroupModel>();
                while (reader.Read())
                {
                    groups.Add(new GroupModel(
                        reader.GetInt32(reader.GetOrdinal("group_id")),
                        reader.GetString(reader.GetOrdinal("owner")),
                        reader.GetString(reader.GetOrdinal("group_name")),
                        reader.GetString(reader.GetOrdinal("icon")),
                        JsonSerializer.Deserialize<List<string>>(reader.GetString(reader.GetOrdinal("features")))
                    ));
                }
                connection.Close();
                result.Groups = groups;
                result.IsSuccessful = true;
                return result;
            }
        }

        public GroupResult CreateGroup(GroupModel group)
        {
            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var result = new GroupResult();

                // Insert SQL statements
                var insertGroupSql = @"INSERT INTO `Groups` (
                                        `owner`, 
                                        `group_name`, 
                                        `icon`,
                                        `features`
                                      )
                                      VALUES (
                                        @owner, 
                                        @group_name, 
                                        @icon,
                                        @features
                                     ); 
                                     SELECT LAST_INSERT_ID();";

                var insertUserGroupSql = @"INSERT INTO `UserGroups` (
                                            `group_id`, 
                                            `username`
                                          )
                                          VALUES (
                                            @group_id, 
                                            @owner 
                                         );";

                var command = connection.CreateCommand();
                command.CommandText = insertGroupSql;
                command.Parameters.AddWithValue("@owner".ToLower(), group.Owner.ToLower());
                command.Parameters.AddWithValue("@group_name".ToLower(), group.GroupName.ToLower());
                command.Parameters.AddWithValue("@icon".ToLower(), group.Icon.ToLower());
                string featuresJSON = JsonSerializer.Serialize(group.Features);
                command.Parameters.AddWithValue("@features", featuresJSON);

                // Execution of first SQL query
                // Storing auto-incremented group_id from insertion into Groups table
                var groupId = command.ExecuteScalar();

                // Group could not be created and could not retrieve group_id primary key
                if (groupId == null)
                {
                    result.IsSuccessful = false;
                    result.Message = "Failure creating group.";
                }
                else
                {
                    // Execution of second SQL query
                    command.CommandText = insertUserGroupSql;
                    command.Parameters.AddWithValue("@group_id", Convert.ToInt32(groupId));
                    var userGroupInsertRows = command.ExecuteNonQuery();
                    group.GroupId = Convert.ToInt32(groupId);
                    result.IsSuccessful = ValidateSqlStatement(userGroupInsertRows).IsSuccessful;
                    result.ReturnedObject = group;
                }
                return result;
            }
        }

        public Result AddGroupMember(GroupModel group, string newGroupMember)
        {
            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var result = new Result();

                // Insert SQL statement
                var insertSql = @"INSERT INTO `UserGroups` (
                                    `group_id`, 
                                    `username`
                                  )
                                  VALUES (
                                    @group_id, 
                                    @username
                                 );";

                var command = connection.CreateCommand();
                command.CommandText = insertSql;
                command.Parameters.AddWithValue("@group_id", group.GroupId);
                command.Parameters.AddWithValue("@username".ToLower(), newGroupMember.ToLower());

                // Execution of SQL
                var rows = (command.ExecuteNonQuery());
                result = ValidateSqlStatement(rows);
                return result;
            }
        }

        public Result RemoveGroupMember(GroupModel group, string groupMember)
        {
            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var result = new Result();

                // Deletion SQL statement
                var insertSql = @"DELETE FROM `UserGroups`
                                  WHERE `group_id` = @group_id 
                                  AND `username` = @username;";

                var command = connection.CreateCommand();
                command.CommandText = insertSql;
                command.Parameters.AddWithValue("@group_id", group.GroupId);
                command.Parameters.AddWithValue("@username".ToLower(), groupMember.ToLower());

                // Execution of SQL
                var rows = (command.ExecuteNonQuery());
                result = ValidateSqlStatement(rows);
                return result;
            }
        }

        //User already exists in group
        public GroupResult FindGroupMember(GroupModel group, string groupMember)
        {
            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var result = new GroupResult();

                // Select SQL statement
                var insertSql = @"SELECT * FROM `UserGroups`
                                    WHERE `username` = @username
                                    AND `group_id` = @group_id;";

                var command = connection.CreateCommand();
                command.CommandText = insertSql;
                command.Parameters.AddWithValue("@group_id", group.GroupId);
                command.Parameters.AddWithValue("@username".ToLower(), groupMember.ToLower());

                // Execution of SQL
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result.ReturnedObject = true;
                    }
                    // User not found in group
                    else
                    {
                        result.ReturnedObject = false;
                    }
                }
                return result;
            }
        }

        public GroupResult GetGroupMembers(GroupModel group)
        {
            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var result = new GroupResult();

                // Select SQL statement
                var deleteSql = @"SELECT * FROM `UserGroups`
                                    WHERE `group_id` = @group_id;";

                var command = connection.CreateCommand();
                command.CommandText = deleteSql;
                command.Parameters.AddWithValue("@group_id", group.GroupId);

                // Execution of SQL
                var groupMembers = new List<string>();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    groupMembers.Add(reader.GetString(reader.GetOrdinal("username")));
                }
                var groupMemberArr = groupMembers.ToArray();
                result.ReturnedObject = groupMemberArr;
                return result;
            }
        }

        public Result DeleteGroup(GroupModel group)
        {
            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var result = new Result();

                // Deletion SQL statement
                var deleteSqlGroup = @"DELETE FROM `Groups`
                                  WHERE `group_id` = @group_id;";

                var deleteSqlUserGroup = @"DELETE FROM `UserGroups`
                                  WHERE `group_id` = @group_id;";

                var command = connection.CreateCommand();
                command.CommandText = deleteSqlGroup;
                command.Parameters.AddWithValue("@group_id", group.GroupId);

                // Execution of SQL
                var rows = (command.ExecuteNonQuery());
                result = ValidateSqlStatement(rows);
                if (result.IsSuccessful)
                {
                    command.CommandText = deleteSqlUserGroup;
                    rows = (command.ExecuteNonQuery());
                    result = ValidateSqlStatement(rows);
                }
                return result;
            }
        }

        public async Task<Result> LogData(Log log)
        {

            _connectionString = BuildConnectionString().ConnectionString;

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Insert SQL statement
                var insertSql = @"INSERT INTO `Logs` 
                                    (`Message`, `Log_Level`, `Category`, `Username`, `Operation`, `Success`) 
                                VALUES 
                                    (@message, @logLevel, @category, @username, @operation, @success);";

                var command = connection.CreateCommand();
                command.CommandText = insertSql;
                command.Parameters.AddWithValue("@message", log.Message);
                command.Parameters.AddWithValue("@logLevel", log.LogLevel);
                command.Parameters.AddWithValue("@category", log.Category);
                command.Parameters.AddWithValue("@username", log.Username);
                command.Parameters.AddWithValue("@operation", log.Operation.ToString());
                command.Parameters.AddWithValue("@success", log.Success);

                // Execution of SQL
                int rows = await command.ExecuteNonQueryAsync();

                connection.Close();

                var result = ValidateSqlStatement(rows);

                return result;
            }
        }

        //Primarily for testing purposes
        public List<Log> GetLogData(UserAccount userAccount, Operations userOperation)
        {
            List<Log> logs = new List<Log>();
            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var result = new Result();

                // Select SQL statement
                var selectSql = @"SELECT * FROM `Logs` 
                                    WHERE `Username` = @username 
                                    AND `Operation` = @operation 
                                    AND `Timestamp` >= NOW() - INTERVAL 1 DAY
                                    ORDER BY `Timestamp` ASC;";

                var command = connection.CreateCommand();
                command.CommandText = selectSql;
                command.Parameters.AddWithValue("@username".ToLower(), userAccount.Username.ToLower());
                command.Parameters.AddWithValue("@operation", userOperation.ToString());

                // Execution of SQL
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var message = reader.GetString(1);
                    LogLevels logLevel = (LogLevels)Enum.Parse(typeof(LogLevels), reader.GetString(2));
                    var category = reader.GetString(3);
                    var timestamp = reader.GetDateTime(4);
                    var username = reader.GetString(5);
                    Operations op = (Operations)Enum.Parse(typeof(Operations), reader.GetString(6));
                    UserOperation operation = new UserOperation(op, 1);
                    logs.Add(new Log(message, logLevel, category, timestamp, username, operation));
                }
                return logs;
            }
        }
    }
}