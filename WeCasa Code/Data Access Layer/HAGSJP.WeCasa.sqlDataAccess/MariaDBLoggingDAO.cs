using Microsoft.Data.SqlClient;
using HAGSJP.WeCasa.Models;
using System.Data;
using MySqlConnector;

namespace HAGSJP.WeCasa.sqlDataAccess
{
    /// <summary>
    /// MariaDB Server Data Access Object aka an object for 
    /// Connecting to MariaDB Server to perform database operations
    /// https://www.nuget.org/packages/MySqlConnector/
    /// </summary>
    public class MariaDBLoggingDAO
    {
        private string _table;
        public MariaDBLoggingDAO() { // default constructor
            _table = "Logs";  
        } 

        public MariaDBLoggingDAO(string table)
        {
            _table = table;
        }

        public Result LogData(string message)
        //public async Task<Result> LogData(string message)
        {
            var result = new Result();

            var builder = new MySqlConnectionStringBuilder
            {
                Server = "localhost",
                Port = 3307,
                UserID = "HAGSJP.WeCasa.SqlUser",
                Password = "cecs491",
                Database = "HAGSJP.WeCasa"
            };

            using (var connection = new MySqlConnection(builder.ConnectionString))
            {
                connection.Open();
                //await connection.OpenAsync();

                // Insert SQL statement
                var insertSql = @"INSERT INTO `Logs` (`Message`) values (@message);";

                var command = connection.CreateCommand();
                command.CommandText = insertSql;
                command.Parameters.AddWithValue("@message", message);

                // Execution of SQL
                var reader = command.ExecuteReader();
                //var reader = command.ExecuteReaderAsync();
                var rows = 0;

                // Reading the results
                while (reader.Read())
                {
                    rows++;
                    var LogId = reader.GetInt32("LogId");
                    var Message = reader.GetString("Message");
                }
                reader.Close();

                connection.Close();

                // Validating results
                if (rows == 1)
                {
                    result.IsSuccessful = true;
                    result.ErrorMessage = string.Empty;

                    return result;
                }
                result.IsSuccessful = false;
                result.ErrorMessage = $"Rows affected were not 1. It was {rows}";

                //connection.Close();

                return result;
            }
        }
    }
}