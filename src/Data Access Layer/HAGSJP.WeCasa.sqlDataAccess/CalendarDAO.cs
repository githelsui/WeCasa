using System;
using System.Configuration;
using System.Text.Json;
using System.Text.RegularExpressions;
using HAGSJP.WeCasa.Models;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Org.BouncyCastle.Crypto;

namespace HAGSJP.WeCasa.sqlDataAccess
{
	public class CalendarDAO : AccountMariaDAO
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

        public async Task<DAOResult> AddEvent(Event e)
		{
            var result = new DAOResult();
            _connectionString = GetConnectionString();
            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();
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
                    command.Parameters.AddWithValue("@created_by", e.CreatedBy);

                    var rows = (command.ExecuteNonQuery());
                    result = ValidateSqlStatement(rows);
                }
                catch (MySqlException sqlex)
                {
                    result.IsSuccessful = false;
                    result.Message = sqlex.Message;
                }
                return result;
            }
		}

        public async Task<DAOResult> UpdateEvent(Event e)
        {
            var result = new DAOResult();
            _connectionString = GetConnectionString();
            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    var updateEventSql =
                        @"UPDATE Events 
                            SET event_name = @event_name, 
                                description = @description, 
                                event_date = @event_date, 
                                repeats = @repeats, 
                                type = @type, 
                                reminder = @reminder, 
                                color = @color
                            WHERE event_id = @event_id;";

                    var command = connection.CreateCommand();
                    command.CommandText = updateEventSql;
                    command.Parameters.AddWithValue("@event_id", e.EventId);
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
                    result.Message = sqlex.Message;
                }
                return result;
            }
        }

        public async Task<DAOResult> DeleteEvent(Event e)
        {
            var result = new DAOResult();
            _connectionString = GetConnectionString();
            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    var deleteEventSql =
                        @"UPDATE `Events` 
                            SET `is_deleted` = 1,
                                `color` = '#ececec'
                            WHERE `event_id` = @event_id;";

                    var command = connection.CreateCommand();
                    command.CommandText = deleteEventSql;
                    command.Parameters.AddWithValue("@event_id", Convert.ToInt32(e.EventId));

                    var rows = (command.ExecuteNonQuery());
                    result = ValidateSqlStatement(rows);
                }
                catch (MySqlException sqlex)
                {
                    result.IsSuccessful = false;
                    result.Message = sqlex.Message;
                }
            }
            return result;
        }

        // Returns all group events in the last year
        public async Task<CalendarResult> GetEvents(int group_id)
        {
            var result = new CalendarResult();
            List<Event> events = new List<Event>();
            _connectionString = GetConnectionString();
            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    var command = connection.CreateCommand();
                    command.CommandText = @"SELECT * FROM Events
                                            WHERE group_id = @group_id
                                            AND ABS(DATEDIFF(event_date, NOW())) <= 365;";

                    command.Parameters.AddWithValue("@group_id", group_id);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Event e = new Event();
                            e.EventId = reader.GetInt32(reader.GetOrdinal("event_id"));
                            e.GroupId = reader.GetInt32(reader.GetOrdinal("group_id"));
                            e.EventName = reader.GetString(reader.GetOrdinal("event_name"));
                            e.Description = reader.GetString(reader.GetOrdinal("description"));
                            e.EventDate = reader.GetDateTime(reader.GetOrdinal("event_date"));
                            e.Repeats = reader.GetString(reader.GetOrdinal("repeats"));
                            e.Type = reader.GetString(reader.GetOrdinal("type"));
                            e.Reminder = reader.GetString(reader.GetOrdinal("reminder"));
                            e.Color = reader.GetString(reader.GetOrdinal("color"));
                            e.IsDeleted = reader.GetInt32(reader.GetOrdinal("is_deleted")) == 1 ? true : false;
                            e.CreatedBy = reader.GetString(reader.GetOrdinal("created_by"));
                            events.Add(e);
                        }
                    }
                    result.IsSuccessful = true;
                    result.ReturnedObject = events;
                    if (events.Count == 0)
                    {
                        result.Message = "No events found.";
                    }
                }
                catch (MySqlException sqlex)
                {
                    result.IsSuccessful = false;
                    result.Message = sqlex.Message;
                }
                return result;
            }
        }
    }
}

