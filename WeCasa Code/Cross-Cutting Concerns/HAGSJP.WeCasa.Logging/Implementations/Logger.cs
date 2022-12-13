using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Logging.Abstractions;
using HAGSJP.WeCasa.sqlDataAccess.Abstractions;
using HAGSJP.WeCasa.Models.Security;

namespace HAGSJP.WeCasa.Logging.Implementations
{
    // Logger object used to record internal system events
    public class Logger : ILogger
    {
        private readonly ILoggerDAO _dao;
        public static string[] Categories = {"view", "business", "server", "data", "data store"};
        public static char[] specialCharacters = {',', '&', '?', '{', '}', '\\', '(', ')', '[', ']', '-', ';', '~', '|', '$', '!', '>', '*', '%', '_'};


        // Dependency inversion principle
        public Logger(ILoggerDAO dao) // Inversion of control
        {
            _dao = dao;
        }

        // Asynchronous method for building and sending logs to the database without including a UserOperation
        public async Task<Result> Log(string message, LogLevel logLevel, string category, string username)
        {             
            var result = new Result();
            #region Step 1: Input validation
            if (message == null)
            {
                result.IsSuccessful = false;
                return result;
            }
            // Check length
            if (message.Length > 200)
            {
                result.IsSuccessful = false;
                result.Message = "Message log too long";
                return result;
            }
            // Check characters
            var matchedIndex = message.IndexOfAny(specialCharacters);
            if (matchedIndex != -1)
            {
                result.IsSuccessful = false;
                result.Message = "Message contains invalid character: " + message[matchedIndex];
                return result;
            }
            // Invalid Log Level
            if (!Enum.IsDefined(typeof(LogLevel), logLevel))
            {
                result.IsSuccessful = false;
                result.Message = "Invalid log level";
                return result;
            }
            // Invalid Category
            if (!Categories.Contains(category.ToLower()))
            {
                result.IsSuccessful = false;
                result.Message = "Invalid category";
                return result;
            }
            #endregion
            Log log = new Log(message, logLevel, category, username);
            var daoResult = await _dao.LogData(log).ConfigureAwait(false);
            if(daoResult.IsSuccessful)
            {
                result.IsSuccessful = true;
                return result;
            }
            result.IsSuccessful = false;
            result.Message = daoResult.Message;
            return result;
        }
        // Asynchronous method for logging a UserOperation
        public async Task<Result> Log(string message, LogLevel logLevel, string category, string username, UserOperation operation)
        {
            var result = new Result();
            #region Step 1: Input validation
            if (message == null)
            {
                result.IsSuccessful = false;
                return result;
            }
            // Check length
            if (message.Length > 200)
            {
                result.IsSuccessful = false;
                result.Message = "Message log too long";
                return result;
            }
            // Check characters
            var matchedIndex = message.IndexOfAny(specialCharacters);
            if (matchedIndex != -1)
            {
                result.IsSuccessful = false;
                result.Message = "Message contains invalid character: " + message[matchedIndex];
                return result;
            }
            // Invalid Log Level
            if (!Enum.IsDefined(typeof(LogLevel), logLevel))
            {
                result.IsSuccessful = false;
                result.Message = "Invalid log level";
                return result;
            }
            // Invalid Category
            if (!Categories.Contains(category.ToLower()))
            {
                result.IsSuccessful = false;
                result.Message = "Invalid category";
                return result;
            }
            #endregion
            Log log = new Log(message, logLevel, category, username, operation);
            var daoResult = await _dao.LogData(log).ConfigureAwait(false);
            if (daoResult.IsSuccessful)
            {
                result.IsSuccessful = true;
                return result;
            }

            result.IsSuccessful = false;
            result.Message = daoResult.Message;
            return result;
        }
    }
}