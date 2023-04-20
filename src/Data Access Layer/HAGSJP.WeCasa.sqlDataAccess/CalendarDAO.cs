using System;
using System.Text.Json;
using System.Text.RegularExpressions;
using HAGSJP.WeCasa.Models;
using MySqlConnector;
using Org.BouncyCastle.Crypto;

namespace HAGSJP.WeCasa.sqlDataAccess
{
	public class CalendarDAO : AccountMariaDAO
	{
        private string _connectionString;
        private DAOResult _result;

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

        public DAOResult AddEvent(Event e)
		{
            var result = new DAOResult();
            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    var insertEventSql =
                        @"INSERT INTO Events (
                                            group_id, 
                                            event_name, 
                                            description, 
                                            event_date, 
                                            repeats, 
                                            type, 
                                            reminder,
                                            color, 
                                            created_by)
                                    VALUES (
                                        @group_id, 
                                        @event_name, 
                                        @description, 
                                        @event_date, 
                                        @repeats, 
                                        @type, 
                                        @reminder, 
                                        @color, 
                                        @created_by);";

                    var command = connection.CreateCommand();
                    command.CommandText = insertEventSql;
                    command.Parameters.AddWithValue("@group_id", e.GroupId);
                    command.Parameters.AddWithValue("@event_name", e.EventName);
                    command.Parameters.AddWithValue("@description", e.Description);
                    command.Parameters.AddWithValue("@event_date", e.EventDate);
                    command.Parameters.AddWithValue("@repeats", e.Repeats);
                    command.Parameters.AddWithValue("@type", e.Type);
                    command.Parameters.AddWithValue("@reminder", e.Reminder);
                    command.Parameters.AddWithValue("@color", e.Color);

                    var rows = (command.ExecuteNonQuery());
                    result = ValidateSqlStatement(rows);
                }
                catch (MySqlException sqlex)
                {
                    result.IsSuccessful = false;
                    result.Message = "An error occurred.";
                }
                return result;
            }
		}

        public void UpdateEvent(Event e)
        {
            
        }

        
        public DAOResult GetEvents(int group_id, DateTime date)
        {
            var result = new DAOResult();
            List<Event> events = new List<Event>();
            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText = @"SELECT * FROM Events
                                            WHERE group_id = @group_id
                                            AND ABS(DATEDIFF(event_date, @event_date)) <= 365;";

                    command.Parameters.AddWithValue("@group_id", group_id);
                    command.Parameters.AddWithValue("@event_date", date);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Event e = new Event();
                            e.GroupId = reader.GetInt32(reader.GetOrdinal("group_id"));
                            e.EventName = reader.GetString(reader.GetOrdinal("event_name"));
                            e.Description = reader.GetString(reader.GetOrdinal("description"));
                            e.EventDate = reader.GetDateTime(reader.GetOrdinal("event_date"));
                            e.Repeats = reader.GetString(reader.GetOrdinal("repeats"));
                            e.Type = reader.GetString(reader.GetOrdinal("type"));
                            e.Reminder = reader.GetString(reader.GetOrdinal("reminder"));
                            e.Color = reader.GetString(reader.GetOrdinal("color"));
                            events.Add(e);
                        }
                        result.ReturnedObject = events;
                    }
                    result.IsSuccessful = true;
                    result.Message = "No events found.";
                }
                catch (MySqlException sqlex)
                {
                    result.IsSuccessful = false;
                    result.Message = "An error occurred.";
                }
                return result;
            }
        }
    }
}

