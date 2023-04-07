using System;
using System.Text.Json;
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

                    var insertSql = @"INSERT INTO Chores (name, group_id, reset_time, notes, assignment, is_repeated, is_completed)
                                    VALUES (@name, @group_id, @reset_time, @notes, @assignment, @is_repeated, @is_completed);
                                    SELECT LAST_INSERT_ID();";

                    var command = connection.CreateCommand();
                    command.CommandText = insertSql;
                    command.Parameters.AddWithValue("@name", chore.Name);
                    command.Parameters.AddWithValue("@group_id", chore.GroupId);
                    command.Parameters.AddWithValue("@reset_time", chore.ResetTime != null ? chore.ResetTime : null);
                    command.Parameters.AddWithValue("@notes", chore.Notes != null ? chore.Notes : null);
                    command.Parameters.AddWithValue("@assignment", chore.Assignment != null ? chore.Assignment : null);
                    command.Parameters.AddWithValue("@is_repeated", chore.IsRepeated == null ? chore.IsRepeated : false);
                    command.Parameters.AddWithValue("@is_completed", chore.IsCompleted == null ? chore.IsCompleted : false);


                    var choreId = Convert.ToInt32(command.ExecuteScalar());
                    if (choreId == null)
                    {
                        result.IsSuccessful = false;
                        result.Message = "Add chore failure";
                    }
                    else
                    {
                        chore.ChoreId = choreId;
                        result.IsSuccessful = true;
                        result.Message = "Add chore success";
                        result.ReturnedObject = chore;
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
                                            reset_time = @reset_time,
                                            notes = @notes,
                                            assignment = @assignment,
                                            is_repeated = @is_repeated,
                                            is_completed = @is_completed
                                    WHERE chore_id = @chore_id";

                    var command = connection.CreateCommand();
                    command.CommandText = updateSql;
                    command.Parameters.AddWithValue("@name", chore.Name);
                    command.Parameters.AddWithValue("@reset_time", chore.ResetTime != null ? chore.ResetTime : null);
                    command.Parameters.AddWithValue("@notes", chore.Notes != null ? chore.Notes : null);
                    command.Parameters.AddWithValue("@assignment", chore.Assignment != null ? chore.Assignment : null);
                    command.Parameters.AddWithValue("@is_repeated", chore.IsRepeated == null ? chore.IsRepeated : false);
                    command.Parameters.AddWithValue("@is_completed", chore.IsCompleted == null ? chore.IsCompleted : false);
                    command.Parameters.AddWithValue("@chore_id", chore.ChoreId);


                    var rows = (command.ExecuteNonQuery());
                    result = result.ValidateSqlResult(rows);
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
                            chore.IsRepeated = reader.GetInt32(reader.GetOrdinal("is_repeated")) == 1 ? true : false;
                            chore.IsCompleted = reader.GetInt32(reader.GetOrdinal("is_completed")) == 1 ? true : false;
                            chore.ResetTime = reader.IsDBNull(reader.GetOrdinal("reset_time")) ? null : reader.GetDateTime(reader.GetOrdinal("reset_time"));
                            chore.Assignment = reader.IsDBNull(reader.GetOrdinal("assignment")) ? "" : reader.GetString(reader.GetOrdinal("assignment"));
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
    }
}

