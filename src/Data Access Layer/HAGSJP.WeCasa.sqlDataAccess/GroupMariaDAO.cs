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
                                    WHERE `username` = @username";
                var command = connection.CreateCommand();
                command.CommandText = selectSql;
                command.Parameters.AddWithValue("@username".ToLower(), userAccount.Username.ToLower());

                // Execution of SQL
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.IsSuccessful = true;
                    result.Message = string.Empty;
                    List<GroupModel> groups = new List<GroupModel>();

                    // User is not a part of any groups 
                    if (!reader.IsDBNull(0))
                    {
                        // creating group from reader object
                        GroupModel group = new GroupModel(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(4), reader.GetDecimal(5));

                        List<string> features = new List<string>();
                        string featuresJSON = reader.GetString(6);
                        // defaulted to all features turned on 
                        if (featuresJSON == "all")
                        {
                            // add all features to list
                        } else
                        {
                            // Deserializing json list of features
                            features = JsonSerializer.Deserialize<List<string>>(featuresJSON);
                        }
                        
                        group.Features = features;

                        // adding list of groups to groups variable
                        groups.Add(group);
                    }
                    // returning the list of groups in our result
                    result.ReturnedObject = groups;
                    return result;
                }
                connection.Close();

                result.IsSuccessful = false;
                result.Message = "An error occurred when fetching groups from the database :(";
                return result;
            }
        }

        public Result CreateGroup(GroupModel group)
        {
            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var result = new Result();

                // Insert SQL statement
                var insertSql = @"INSERT INTO `Groups` (
                                    `group_id`, 
                                    `owner`, 
                                    `group_name`, 
                                    `username`, 
                                    `budget`, 
                                    `features`
                                  )
                                  VALUES (
                                    @group_id, 
                                    @owner, 
                                    @group_name, 
                                    @username, 
                                    @budget, 
                                    @features
                                 );";

                var command = connection.CreateCommand();
                command.CommandText = insertSql;
                command.Parameters.AddWithValue("@group_id", group.GroupId);
                command.Parameters.AddWithValue("@owner".ToLower(), group.Owner.ToLower());
                command.Parameters.AddWithValue("@group_name".ToLower(), group.GroupName.ToLower());
                command.Parameters.AddWithValue("@username".ToLower(), group.Owner.ToLower());
                command.Parameters.AddWithValue("@budget", group.Budget);
                string featuresJSON = JsonSerializer.Serialize(group.Features);
                command.Parameters.AddWithValue("@features", featuresJSON);

                // Execution of SQL
                var rows = (command.ExecuteNonQuery());
                result = ValidateSqlStatement(rows);
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