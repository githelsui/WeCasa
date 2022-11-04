using Microsoft.Data.SqlClient;
using HAGSJP.WeCasa.Models;

namespace HAGSJP.WeCasa.sqlDataAccess
{
    /// <summary>
    /// SQL Server Data Access Object aka an object for 
    /// connecting to MS SQL Server to perform database operations
    /// </summary>
    public class LoggingDAO
    {
        private string _connectionString; // backing field

        public LoggingDAO(string connectionString)
        {
            // storing in private string is not 100% secure, use SecureString()
            // or encryption/decryption
            _connectionString = connectionString;
        }

        public Result LogData(string message)
        {
            var result = new Result();
            // connection strings sql Server: standard security
      
            var connectionString = @"Server=.\;Database=HAGSJPWeCasa.Logs;Integrated Security=True;";
 
            using (var connection = new SqlConnection(connectionString)) // ADO.NET, all relational DB accept ANSI SQL
            {
                connection.Open();

                // Insert SQL statement
                var insertSql = "INSERT INTO HAGSJPWeCasa.Logs (Message) values(%message)";
                var command = new SqlCommand(insertSql, connection);

                // Execution of SQL
                var rows = command.ExecuteNonQuery();

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