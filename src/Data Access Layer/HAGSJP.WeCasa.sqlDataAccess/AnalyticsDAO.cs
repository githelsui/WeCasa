using System;
using System.Data.SqlTypes;
using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;
using HAGSJP.WeCasa.Models;
using MySqlConnector;

namespace HAGSJP.WeCasa.sqlDataAccess
{
	public class AnalyticsDAO
	{
        private string _connectionString;
        private DAOResult result;

        public AnalyticsDAO() { }

        public MySqlConnectionStringBuilder BuildConnectionString()
        {
            var builder = new MySqlConnectionStringBuilder
            {
                Server = "54.218.41.23",
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

        public DAOResult GetLoginsPerDay(DateTime minDate)
        {
            var result = new DAOResult();
            var loginsPerDate = new List<Dictionary<String, String>>();
            _connectionString = BuildConnectionString().ConnectionString;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                var selectSql = @"SELECT CAST(Timestamp AS DATE) as DATE_ONLY , COUNT(*) as NUM_ROWS
                                    FROM Logs
                                    WHERE Message = @message
                                    AND Timestamp >= @min_date
                                    AND Timestamp <= NOW()
                                    GROUP BY CAST(Timestamp AS DATE);";

                var command = connection.CreateCommand();
                command.CommandText = selectSql;
                command.Parameters.AddWithValue("@operation", "Login");
                command.Parameters.AddWithValue("@min_date", minDate);
                command.Parameters.AddWithValue("@message", "Login successful");

                // Execution of SQL
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    DateTime date = reader.GetDateTime(reader.GetOrdinal("DATE_ONLY")).Date;
                    var dateStr = date.ToShortDateString();
                    int logins = reader.GetInt32(reader.GetOrdinal("NUM_ROWS"));
                    var loginsStr = logins.ToString();
                    var dateLogin = new Dictionary<String, String>
                    {
                        { "date", dateStr},
                        { "value", loginsStr }
                    };
                    loginsPerDate.Add(dateLogin);
                }
                result.ReturnedObject = loginsPerDate;
                result.IsSuccessful = true;
                return result;
            }
            result.IsSuccessful = false;
            result.Message = "Failed fetching logins per day"; 
            return result;
        }
    }
}

