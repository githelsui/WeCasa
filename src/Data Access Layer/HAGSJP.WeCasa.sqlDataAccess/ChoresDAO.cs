using System;
using System.Text.Json;
using System.Text.RegularExpressions;
using HAGSJP.WeCasa.Models;
using MySqlConnector;

namespace HAGSJP.WeCasa.sqlDataAccess
{
    public class ChoresDAO : AccountMariaDAO
    {
        private string _connectionString;
        private DAOResult result;

        public ChoresDAO() { }

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

        public DAOResult ValidateSqlStatement(int rows)
        {
            var result = new DAOResult();

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

        public DAOResult ValidateInsertStatements(int rows)
        {
            var result = new DAOResult();

            if (rows > 0)
            {
                result.IsSuccessful = true;
                result.Message = string.Empty;

                return result;
            }
            result.IsSuccessful = false;
            result.Message = $"Rows affected were 0. Sql insert unsuccessful.";

            return result;
        }

        public DAOResult CreateChore(Chore chore)
        {
            var result = new DAOResult();
            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    var insertChoreSql = @"INSERT INTO Chores (name, group_id, notes, assigned_to, repeats, is_completed, created, created_by, days)
                                    VALUES (@name, @group_id, @notes, @assigned_to, @repeats, @is_completed, @created, @created_by, @days);
                                    SELECT LAST_INSERT_ID();";

                    var command = connection.CreateCommand();
                    command.CommandText = insertChoreSql;
                    command.Parameters.AddWithValue("@name", chore.Name);
                    command.Parameters.AddWithValue("@group_id", chore.GroupId);
                    command.Parameters.AddWithValue("@created", chore.Created != null ? chore.Created : DateTime.Now);
                    command.Parameters.AddWithValue("@created_by", chore.CreatedBy);
                    command.Parameters.AddWithValue("@notes", chore.Notes != null ? chore.Notes : null);
                    command.Parameters.AddWithValue("@repeats", chore.Repeats != null ? chore.Repeats : null);
                    command.Parameters.AddWithValue("@is_completed", chore.IsCompleted == null || chore.IsCompleted == false ? 0 : 1);
                    string assignedToJSON = JsonSerializer.Serialize(chore.AssignedTo);
                    command.Parameters.AddWithValue("@assigned_to", assignedToJSON);
                    string daysJson = JsonSerializer.Serialize(chore.Days);
                    command.Parameters.AddWithValue("@days", daysJson);

                    // Execution of first query for Chore table
                    var choreId = Convert.ToInt32(command.ExecuteScalar());
                    if (choreId == null)
                    {
                        result.IsSuccessful = false;
                        result.Message = "Failure creating chore.";
                    }
                    else // First query successful
                    {
                        chore.ChoreId = choreId;
                        result = AssignChore(chore);
                        if (result.IsSuccessful)
                        {
                            result.IsSuccessful = true;
                            result.Message = "Creating chore success.";
                            result.ReturnedObject = chore;
                            return result;
                        }
                        else
                        {
                            result.IsSuccessful = false;
                            result.Message += "Error assigning chore to users.";
                        }
                    }
                }
                catch (MySqlException sqlex)
                {
                    throw sqlex;
                }
                catch (Exception sqlex)
                {
                    throw sqlex;
                }
                return result;
            }
        }

        public DAOResult UpdateChore(Chore chore)
        {
            // Does not handle updates to chore assignments
            var result = new DAOResult();
            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    var updateSql = @"UPDATE Chores
                                        SET
                                            name = @name,
                                            last_updated = @last_updated,
                                            last_updated_by = @last_updated_by,
                                            assigned_to = @assigned_to,
                                            notes = @notes,
                                            days = @days,
                                            repeats = @repeats,
                                    WHERE chore_id = @chore_id;";

                    var command = connection.CreateCommand();
                    command.CommandText = updateSql;
                    command.Parameters.AddWithValue("@name", chore.Name);
                    command.Parameters.AddWithValue("@last_updated", chore.LastUpdated);
                    command.Parameters.AddWithValue("@last_updated_by", chore.LastUpdatedBy);
                    command.Parameters.AddWithValue("@notes", chore.Notes != null ? chore.Notes : null);
                    command.Parameters.AddWithValue("@repeats", chore.Repeats != null ? chore.Repeats : null);
                    command.Parameters.AddWithValue("@chore_id", chore.ChoreId);
                    string assignedToJSON = JsonSerializer.Serialize(chore.AssignedTo);
                    command.Parameters.AddWithValue("@assigned_to", assignedToJSON);
                    string daysJson = JsonSerializer.Serialize(chore.Days);
                    command.Parameters.AddWithValue("@days", daysJson);

                    var rows = (command.ExecuteNonQuery());
                    result = ValidateInsertStatements(rows);
                    if (result.IsSuccessful)
                    {
                        result = ReassignChore(chore);
                        if (result.IsSuccessful)
                        {
                            return result;
                        }
                    }
                    result.IsSuccessful = false;
                    result.Message += "Failed to update chore. ";
                    return result;
                }
                catch (MySqlException sqlex)
                {
                    throw sqlex;
                }
                catch (Exception sqlex)
                {
                    throw sqlex;
                }
            }
        }

        public DAOResult CompleteChore(Chore chore)
        {
            var result = new DAOResult();
            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    var updateSql = @"UPDATE Chores
                                        SET
                                            last_completed = @last_completed,
                                            is_completed = @is_completed
                                    WHERE chore_id = @chore_id;";

                    var command = connection.CreateCommand();
                    command.CommandText = updateSql;
                    command.Parameters.AddWithValue("@last_completed", chore.LastCompleted);
                    command.Parameters.AddWithValue("@is_completed", (chore.IsCompleted == null || chore.IsCompleted == false) ? 0 : 1);
                    command.Parameters.AddWithValue("@chore_id", chore.ChoreId);

                    var rows = (command.ExecuteNonQuery());
                    result = ValidateSqlStatement(rows);
                    return result;
                }
                catch (MySqlException sqlex)
                {
                    throw sqlex;
                }
                catch (Exception sqlex)
                {
                    throw sqlex;
                }
            }
        }

        public DAOResult AssignChore(Chore chore)
        {
            // Reset chore.AssignedTo for Chores table & Usergroups table
            var result = new DAOResult();
            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();


                    var command = connection.CreateCommand();
                    // Creates new assignments in UserChore table
                    var valuesStr = "";
                    for (var i = 0; i < chore.AssignedTo.Count; i++)
                    {
                        string username = chore.AssignedTo[i].Username;
                        if (i == chore.AssignedTo.Count - 1)
                        {
                            valuesStr += $"({chore.ChoreId}, '{username}', 0)";
                        }
                        else
                        {
                            valuesStr += $"({chore.ChoreId}, '{username}', 0), ";
                        }
                    }

                    var insertSql = string.Format(@"INSERT INTO UserChore (chore_id, username, is_completed) VALUES {0}", valuesStr);
                    Console.Write(insertSql);
                    command.CommandText = insertSql;
                    var rows = (command.ExecuteNonQuery());
                    result = ValidateInsertStatements(rows);
                    if (result.IsSuccessful)
                    {
                        result.Message = "Assigning chore success.";
                        result.ReturnedObject = chore;
                    }
                    else
                    {
                        result.Message += "Failure assigning chore. ";
                    }
                }
                catch (MySqlException sqlex)
                {
                    throw sqlex;
                }
                catch (Exception sqlex)
                {
                    throw sqlex;
                }
                return result;
            }
        }

        public DAOResult ReassignChore(Chore chore)
        {
            // Reset chore.AssignedTo for Chores table & Usergroups table
            var result = new DAOResult();
            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    var resetSql = @"DELETE FROM UserChore
                                     WHERE chore_id = @chore_id;";

                    var command = connection.CreateCommand();
                    command.CommandText = resetSql;
                    command.Parameters.AddWithValue("@chore_id", chore.ChoreId);

                    // Execution of first query: Resets old assignments if old assignments exist in UserChore table
                    var resetSqlRows = command.ExecuteNonQuery();
                    var resetSqlRowsResult = ValidateInsertStatements(resetSqlRows);
                    if (!resetSqlRowsResult.IsSuccessful)
                    {
                        result.IsSuccessful = false;
                        result.Message += "Failure reseting chore assignments. ";
                    }
                    else // First query successful
                    {
                        // Execution of second query: Create new assignments in UserChore
                        result = AssignChore(chore);
                        if (result.IsSuccessful)
                        {
                            result.IsSuccessful = true;
                            result.Message = "Assigning chore success.";
                            result.ReturnedObject = chore;
                        }
                        else
                        {
                            result.IsSuccessful = false;
                            result.Message += "Failure assigning chore. ";
                        }
                    }
                }
                catch (MySqlException sqlex)
                {
                    throw sqlex;
                }
                catch (Exception sqlex)
                {
                    throw sqlex;
                }
                return result;
            }
        }

        public DAOResult DeleteChore(Chore chore)
        {
            var result = new DAOResult();
            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    var deleteChoreSql = @"DELETE FROM Chores
                                     WHERE chore_id = @chore_id;";

                    var command = connection.CreateCommand();
                    command.CommandText = deleteChoreSql;
                    command.Parameters.AddWithValue("@chore_id", chore.ChoreId);

                    // Execution of first query
                    var resetSqlRows = command.ExecuteNonQuery();
                    var resetSqlRowsResult = ValidateInsertStatements(resetSqlRows);
                    if (!resetSqlRowsResult.IsSuccessful)
                    {
                        result.IsSuccessful = false;
                        result.Message += "Failure deleting chore.";
                    }
                    else // First query successful
                    {
                        // Execution of second query
                        var deleteUserChoreSql = @"DELETE FROM UserChore
                                     WHERE chore_id = @chore_id;";
                        command = connection.CreateCommand();
                        command.CommandText = deleteUserChoreSql;
                        command.Parameters.AddWithValue("@chore_id", chore.ChoreId);
                        resetSqlRows = command.ExecuteNonQuery();
                        resetSqlRowsResult = ValidateInsertStatements(resetSqlRows);
                        if (resetSqlRowsResult.IsSuccessful)
                        {
                            result.IsSuccessful = true;
                            result.Message = "Deleting chore assignments success.";
                            result.ReturnedObject = chore;
                        }
                        else
                        {
                            result.IsSuccessful = false;
                            result.Message += "Failure deleting chore assignments. ";
                        }
                    }
                }
                catch (MySqlException sqlex)
                {
                    throw sqlex;
                }
                catch (Exception sqlex)
                {
                    throw sqlex;
                }
                return result;
            }
        }

        public DAOResult GetChores(string selectSql)
        {
            var result = new DAOResult();
            List<Chore> chores = new List<Chore>();
            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText = selectSql;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Chore chore = new Chore();
                            chore.ChoreId = reader.GetInt32(reader.GetOrdinal("chore_id"));
                            chore.GroupId = reader.GetInt32(reader.GetOrdinal("group_id"));
                            chore.Name = reader.GetString(reader.GetOrdinal("name"));
                            chore.CreatedBy = reader.IsDBNull(reader.GetOrdinal("created_by")) ? "" : reader.GetString(reader.GetOrdinal("created_by"));
                            chore.Created = reader.IsDBNull(reader.GetOrdinal("created")) ? null : reader.GetDateTime(reader.GetOrdinal("created"));
                            chore.LastUpdatedBy = reader.IsDBNull(reader.GetOrdinal("last_updated_by")) ? "" : reader.GetString(reader.GetOrdinal("last_updated_by"));
                            chore.LastUpdated = reader.IsDBNull(reader.GetOrdinal("last_updated")) ? null : reader.GetDateTime(reader.GetOrdinal("last_updated"));
                            chore.Notes = reader.IsDBNull(reader.GetOrdinal("notes")) ? "" : reader.GetString(reader.GetOrdinal("notes"));
                            chore.Repeats = reader.IsDBNull(reader.GetOrdinal("repeats")) ? "" : reader.GetString(reader.GetOrdinal("repeats"));
                            chore.IsCompleted = reader.GetInt32(reader.GetOrdinal("is_completed")) == 1 ? true : false;
                            List<UserProfile>? assignedTo = JsonSerializer.Deserialize<List<UserProfile>>(reader.GetString(reader.GetOrdinal("assigned_to")));
                            chore.AssignedTo = assignedTo;
                            List<String>? days = reader.IsDBNull(reader.GetOrdinal("days")) ? new List<String>() : JsonSerializer.Deserialize<List<String>>(reader.GetString(reader.GetOrdinal("days")));
                            chore.Days = days;
                            chores.Add(chore);
                        }
                        result.IsSuccessful = true;
                        result.ReturnedObject = chores;
                        return result;
                    }
                    result.IsSuccessful = false;
                    result.Message = "Cannot find chores.";
                    return result;

                }
                catch (MySqlException sqlex)
                {
                    throw sqlex;
                }
                catch (Exception sqlex)
                {
                    throw sqlex;
                }
            }
        }

        public DAOResult GetUserChores(string selectSql)
        {
            var result = new DAOResult();
            List<int> choreIds = new List<int>();
            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText = selectSql;

                    // Execution of first sql query for UserChore table
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var choreId = reader.GetInt32(reader.GetOrdinal("chore_id"));
                            choreIds.Add(choreId);
                        }
                    }

                    if (choreIds.Count > 0)
                    {
                        result.ReturnedObject = choreIds;
                        result.IsSuccessful = true;
                    }
                    else
                    {
                        result.ReturnedObject = new List<Chore>();
                        result.IsSuccessful = true;
                        result.Message = "User has no chores under these specifications.";
                    }
                    return result;
                }
                catch (MySqlException sqlex)
                {
                    throw sqlex;
                }
                catch (Exception sqlex)
                {
                    throw sqlex;
                }
            }
        }
        public async Task<ChoreResult> GetUserProgress(string username, int group_id)
        {
            var result = new ChoreResult();
            var progressReport = new ProgressReport(group_id, username);
            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    var command = connection.CreateCommand();
                    var selectSql = @"SELECT COUNT(CASE WHEN uc.is_completed = 1 THEN 1 ELSE NULL END) AS completedChores,
                                         COUNT(CASE WHEN uc.is_completed = 0 THEN 1 ELSE NULL END) AS incompleteChores
                                      FROM userchore AS uc
                                         INNER JOIN chores AS c 
                                            ON (uc.chore_id = c.chore_id)
                                      WHERE username = @username 
                                            AND group_id = @group_id
                                            AND MONTH(c.created) = MONTH(NOW());";
                    
                    command.CommandText = selectSql;
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@group_id", group_id);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var completedChores = reader.GetInt32(reader.GetOrdinal("completedChores"));
                            var incompleteChores = reader.GetInt32(reader.GetOrdinal("incompleteChores"));
                            progressReport.CompletedChores = completedChores;
                            progressReport.IncompleteChores = incompleteChores;
                        }
                        result.ReturnedObject = progressReport;
                        result.IsSuccessful = true;
                    }
                }
                catch (MySqlException sqlex)
                {
                    result.Message = sqlex.Message;
                }
                catch (Exception sqlex)
                {
                    result.Message = sqlex.Message;
                }
                return result;
            }
        }

    }
}

