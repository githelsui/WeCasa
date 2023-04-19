﻿using System;
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

        public DAOResult CreateChore(Chore chore)
		{
            var result = new DAOResult();
            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    var insertChoreSql = @"INSERT INTO Chores (name, group_id, reset_time, notes, assigned_to, repeats, is_completed, created, created_by)
                                    VALUES (@name, @group_id, @reset_time, @notes, @assigned_to, @repeats, @is_completed, @created, @created_by);
                                    SELECT LAST_INSERT_ID();";

                    var command = connection.CreateCommand();
                    command.CommandText = insertChoreSql;
                    command.Parameters.AddWithValue("@name", chore.Name);
                    command.Parameters.AddWithValue("@group_id", chore.GroupId);
                    command.Parameters.AddWithValue("@created", chore.Created);
                    command.Parameters.AddWithValue("@created_by", chore.CreatedBy);
                    command.Parameters.AddWithValue("@reset_time", chore.ResetTime != null ? chore.ResetTime : null);
                    command.Parameters.AddWithValue("@notes", chore.Notes != null ? chore.Notes : null);
                    command.Parameters.AddWithValue("@repeats", chore.Repeats != null ? chore.Repeats : null);
                    command.Parameters.AddWithValue("@is_completed", chore.IsCompleted == null ? chore.IsCompleted : false);
                    string assignedToJSON = JsonSerializer.Serialize(chore.AssignedTo);
                    command.Parameters.AddWithValue("@assigned_to", assignedToJSON);

                    // Execution of first query for Chore table
                    var choreId = Convert.ToInt32(command.ExecuteScalar());
                    if (choreId == null)
                    {
                        result.IsSuccessful = false;
                        result.Message = "Failure creating chore.";
                    }
                    else // First query successful
                    {
                        result = AssignChores(chore);
                        if (result.IsSuccessful)
                        {
                            chore.ChoreId = choreId;
                            result.IsSuccessful = true;
                            result.Message = "Creating chore success.";
                            result.ReturnedObject = chore;
                            return result;
                        }
                        else
                        {
                            result.IsSuccessful = false;
                            result.Message = "Failure creating chore. Error assigning chore to users.";
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
                                            reset_time = @reset_time,
                                            notes = @notes,
                                            repeats = @repeats,
                                            is_completed = @is_completed
                                    WHERE chore_id = @chore_id;";

                    var command = connection.CreateCommand();
                    command.CommandText = updateSql;
                    command.Parameters.AddWithValue("@name", chore.Name);
                    command.Parameters.AddWithValue("@last_updated", chore.LastUpdated);
                    command.Parameters.AddWithValue("@last_updated_by", chore.LastUpdatedBy);
                    command.Parameters.AddWithValue("@reset_time", chore.ResetTime != null ? chore.ResetTime : null);
                    command.Parameters.AddWithValue("@notes", chore.Notes != null ? chore.Notes : null);
                    command.Parameters.AddWithValue("@repeats", chore.Repeats != null ? chore.Repeats : null);
                    command.Parameters.AddWithValue("@is_completed", chore.IsCompleted == null ? chore.IsCompleted : false);
                    command.Parameters.AddWithValue("@chore_id", chore.ChoreId);
                    string assignedToJSON = JsonSerializer.Serialize(chore.AssignedTo);
                    command.Parameters.AddWithValue("@assigned_to", assignedToJSON);

                    var rows = (command.ExecuteNonQuery());
                    result = result.ValidateSqlResult(rows);
                    if (result.IsSuccessful)
                    {
                        result = AssignChores(chore);
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

        public DAOResult AssignChores(Chore chore)
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
                    var resetSqlRowsResult = ValidateSqlStatement(resetSqlRows);
                    if (!resetSqlRowsResult.IsSuccessful)
                    {
                        result.IsSuccessful = false;
                        result.Message += "Failure reseting chore assignments. ";
                    }
                    else // First query successful
                    {
                        // Execution of second query: Creates new assignments 
                        var valuesStr = "";
                        for (var i = 0; i < chore.AssignedTo.Count; i++)
                        {
                            string username = chore.AssignedTo[i].Username;
                            if (i == chore.AssignedTo.Count - 1)
                            {
                                valuesStr += $"({chore.ChoreId}, {username}, 0, 1);";
                            }
                            else
                            {
                                valuesStr += $"({chore.ChoreId}, {username}, 0, 1), ";
                            }
                        }
                        var insertUserChoreSql = string.Format(@"INSERT INTO UserChore (chore_id, username, is_completed, is_assigned) VALUES '{0}'", valuesStr);
                        command.CommandText = insertUserChoreSql;
                        var userChoreInsertRows = command.ExecuteNonQuery();
                        var userChoreResult = ValidateSqlStatement(userChoreInsertRows);
                        if (userChoreResult.IsSuccessful)
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
                            chore.Notes = reader.IsDBNull(reader.GetOrdinal("notes")) ? "" : reader.GetString(reader.GetOrdinal("notes"));
                            command.Parameters.AddWithValue("@repeats", chore.Repeats != null ? chore.Repeats : null);
                            chore.IsCompleted = reader.GetInt32(reader.GetOrdinal("is_completed")) == 1 ? true : false;
                            chore.ResetTime = reader.IsDBNull(reader.GetOrdinal("reset_time")) ? null : reader.GetDateTime(reader.GetOrdinal("reset_time"));
                            List<UserProfile>? assignedTo = JsonSerializer.Deserialize<List<UserProfile>>(reader.GetString(reader.GetOrdinal("assigned_to")));
                            chore.AssignedTo = assignedTo;
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

                    if(choreIds.Count > 0)
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
    }
}
