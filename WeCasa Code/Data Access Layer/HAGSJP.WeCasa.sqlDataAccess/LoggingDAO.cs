using Microsoft.Data.SqlClient;
using HAGSJP.WeCasa.Models;
using System.Data;

namespace HAGSJP.WeCasa.sqlDataAccess
{
    /// <summary>
    /// SQL Server Data Access Object aka an object for 
    /// connecting to MS SQL Server to perform database operations
    /// </summary>
    public class LoggingDAO
    {
        private string _connectionString; // backing field

        public LoggingDAO() // default constructor
        {
            _connectionString = @"Server=.\;Database=HAGSJP.WeCasa;Integrated Security=True;TrustServerCertificate=True";
        }

        public LoggingDAO(string connectionString)
        {
            // storing in private string is not 100% secure, use SecureString()
            // or encryption/decryption
            _connectionString = connectionString;
        }

        public Result LogData(string message)
        {
            var result = new Result();
             
            using (var connection = new SqlConnection(_connectionString)) // ADO.NET, all relational DB accept ANSI SQL
            {
                connection.Open();

                // Insert SQL statement
                var insertSql = "INSERT INTO WeCasaLogs (Message) values(@message)";
                var command = new SqlCommand(insertSql, connection);
                command.Parameters.Add("@message", SqlDbType.VarChar).Value = message;

                // Execution of SQL
                var rows = command.ExecuteNonQuery();

                connection.Close();

                if (rows == 1)
                {
                    result.IsSuccessful = true;
                    result.ErrorMessage = string.Empty;
                    return result;
                }
                result.IsSuccessful = false;
                result.ErrorMessage = $"Rows affected were not 1. It was {rows}";
                return result;
            }
        }
    }
}