using HAGSJP.WeCasa.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;

namespace HAGSJP.WeCasa.sqlDataAccess
{
    /// <summary>
    /// SQL Server Data Access Object aka an object for 
    /// connecting to MS SQL Server to perform database operations
    /// </summary>
    public class SqlDAO
    {
        private string _connectionString; // backing field

        public enum ResultStatus
        {
            Unknown = 0,
            Success = 1,
            Faulty = 2,
        }

        public SqlDAO(string connectionString)
        {
            // storing in private string is not 100% secure, use SecureString()
            // or encryption/decryption
            _connectionString = connectionString;
        }


        public Result ExecuteSql(string sql)
        {
            var result = new Result();
            // connection strings sql Server: standard security
            using (var connection = new SqlConnection(_connectionString)) // ADO.NET, all relational DB accept ANSI SQL
            {
                connection.Open();

                // Insert SQL statement
                var insertSql = "INSERT INTO HAGSJP.WeCasa.Logs (Message) values(%message)";

                var command = new SqlCommand(insertSql, connection);
                
                // Execution of SQL
                var rows = command.ExecuteNonQuery();

                if (rows == 1)
                {
                    result.IsSuccessful = ResultStatus.Success;
                    result.ErrorMessage = string.Empty;
                    return result;
                }
                result.IsSuccessful = ResultStatus.Faulty;
                result.ErrorMessage = $"Rows affected were not 1. It was {rows}";
                return result;
            }
        }
    }
}