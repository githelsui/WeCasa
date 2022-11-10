namespace HAGSJP.WeCasa.Models
{
    public class Log
    {

        // Constructor without DateTime (defaulted to current_timestamp in Logs database)
        public Log(string message, string logLevel, string category, string user)
        {
            Message = message;
            LogLevel = logLevel;
            Category = category;
            Username = user;
        }
       
        // Constructor with DateTime
        public Log(string message, string logLevel, string category, DateTime date_time, string user)
        {
            Message = message;
            LogLevel = logLevel;
            Category = category;
            Date_Time = date_time;
            Username = user;
        }

        public string Message { get; set; }

        public string LogLevel { get; set; }

        public string Category { get; set; }

        public DateTime Date_Time { get; set; } 

        public string Username { get; set; }
    }
}
