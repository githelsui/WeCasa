using Microsoft.VisualBasic;
using Microsoft.Data.SqlClient;

namespace HAGSJP.WeCasa.Logging.Implementations
{

    public enum ResultStatus
    {
        Unknown = 0
        Success = 1,
        Faulty = 2,
    }

    public class Result
    {
        public ResultStatus IsSuccessful { get; set; }

        public string ErrorMessage { get; set; }

        //public object Payload { get; set; }
    }


    public class DatabaseLogger
    {
        public DatabaseLogger()
        {

        }

        public DatabaseLogger(String tableName)
        {

        }

        public Result Log(String message)
        {
            // TODO: Log to database
            // Concerns: security vulnerability, only returns bool,
            var result = new Result();

            // Step 1: Input Validation (include bool AND feedback for what went wrong)
            #region Step 1: Input validation
            if (message == null)
            {
                result.IsSuccessful = true;
                return true;
            }

            if (message.Length > 200)
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Message log too long";
                return false;
            }
            // invalid characters
            if (message.Contains("<"))
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Mesage contians < which is invalid";
                return false;
            }
            #endregion

            // Step 2: Perform the logging
            // database server(IP address w/ port), database, table, coloumn

            // connect to database (download Microsoft.Data.SqlClient)
            // SQL Server -> T-SQL
            // connection strings sql Server: standard security
            // IN DEV
            var connectionString = @"Server=.\;Database=Company.Product.Logs;Integrated Security=True;";
            // IN PROD
            //var connectionString = @"Server=.\;Database=Company.Product.Logs;User Id=myUsername;Password=myPassword;Encrypt=True;";
            using (var connection = new SqlConnection(connectionString)) // ADO.NET, all relational DB accept ANSI SQL
            {
                connection.open();

                // Insert SQL statement
                //var insertSql = "INSERT INTO Company.Product.Logs (Message) values(" + message +")";
                var insertSql = "INSERT INTO Company.Product.Logs (Message) values(%message)";
                var command = new SqlCommand(insertSql, connection);
                var parameter = new SqlParameter("message", message);
                command.Parameters.Add(parameter);
                // Execution of SQL
                var rows = command.ExecuteNonQuery();

                if (rows == 1)
                {
                    result.IsSuccessful = true;
                    result.ErrorMessage = string.Empty;
                    return true;
                }
                result.IsSuccessful = false;
                result.ErrorMessage = $"Rows affected were not 1. It was {rows}";
                return false;
            }

        }
    }
}