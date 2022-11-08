using Microsoft.Data.SqlClient;
using HAGSJP.WeCasa.Models;
using System.Data;
using MySqlConnector;

namespace HAGSJP.WeCasa.sqlDataAccess
{
    /// <summary>
    /// SQL Server Data Access Object aka an object for 
    /// Connecting to MS SQL Server to perform database operations
    /// https://www.nuget.org/packages/MySqlConnector/
    /// </summary>
    public class SQLServerLoggingDAO
    {
        private string _connectionString; // Backing field

        public SQLServerLoggingDAO() // Default constructor
        { 
            _connectionString = @"Server=.\;Database=HAGSJP.WeCasa;Integrated Security=True;TrustServerCertificate=True";
        }

        public SQLServerLoggingDAO(string connectionString)
        {
            // Storing in private string is not 100% secure, use SecureString()
            // or encryption/decryption
            _connectionString = connectionString;
        }

        public async Task<Result> LogData(string message)
        {
            var result = new Result();

            using (var connection = new SqlConnection(_connectionString)) // ADO.NET, all relational DB accept ANSI SQL
            {
                await connection.OpenAsync();

                // Insert SQL statement
                var insertSql = "INSERT INTO Logs (Message) values(@message)";
                var command = new SqlCommand(insertSql, connection);

                command.CommandText = insertSql;
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