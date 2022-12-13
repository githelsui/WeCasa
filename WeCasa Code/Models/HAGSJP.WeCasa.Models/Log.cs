namespace HAGSJP.WeCasa.Models
{
    public class Log
    {

        // Constructor without DateTime (defaulted to current_timestamp in Logs database)
        public Log(string message, LogLevel logLevel, string category, string user)
        {
            Message = message;
            LogLevel = logLevel.ToString();
            Category = category;
            Username = user;
        }

        // Constructor without DateTime, with UserOperation
        public Log(string message, LogLevel logLevel, string category, string user, UserOperation uo)
        {
            Message = message;
            LogLevel = logLevel.ToString();
            Category = category;
            Username = user;
            Operation = uo;
        }

        // Constructor with DateTime
        public Log(string message, LogLevel logLevel, string category, DateTime date_time, string user, UserOperation uo)
        {
            Message = message;
            LogLevel = logLevel.ToString();
            Category = category;
            Date_Time = date_time;
            Username = user;
            Operation = uo;
        }

        public string Message { get; set; }

        public string LogLevel { get; set; }

        public string Category { get; set; }

        public DateTime Date_Time { get; set; } 

        public string Username { get; set; }
        public UserOperation Operation { get; set; }
    }
}
