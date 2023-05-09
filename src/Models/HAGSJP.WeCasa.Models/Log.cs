using System.Text.Json.Serialization;

namespace HAGSJP.WeCasa.Models
{
    public class Log
    {

        [JsonConstructor]
        public Log()
        {
        }

        // Constructor without DateTime (defaulted to current_timestamp in Logs database) or UserOperation
        // For logging internal system information (not user interactions)
        public Log(string message, LogLevels logLevel, string category, string user)
        {
            Message = message;
            LogLevel = logLevel.ToString();
            Category = category;
            Username = user;
        }

        // Constructor without DateTime (defaulted to current_timestamp in Logs database), with UserOperation, success
        public Log(string message, LogLevels logLevel, string category, string user, UserOperation userOperation)
        {
            Message = message;
            LogLevel = logLevel.ToString();
            Category = category;
            Username = user;
            Operation = userOperation.Operation;
            Success = userOperation.Success;
        }

        // Complete Constructor with DateTime
        public Log(string message, LogLevels logLevel, string category, DateTime date_time, string user, UserOperation userOperation)
        {
            Message = message;
            LogLevel = logLevel.ToString();
            Category = category;
            Date_Time = date_time;
            Username = user;
            Operation = userOperation.Operation;
            Success = userOperation.Success;
        }

        public string Message { get; set; }

        public string LogLevel { get; set; }

        public string Category { get; set; }

        public DateTime Date_Time { get; set; } 
        public string Username { get; set; }
        public Operations? Operation { get; set; }
        public int? Success { get; set; }
    }
}
