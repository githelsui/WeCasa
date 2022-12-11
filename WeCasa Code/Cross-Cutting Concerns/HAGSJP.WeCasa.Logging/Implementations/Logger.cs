using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Logging.Abstractions;
using HAGSJP.WeCasa.sqlDataAccess.Abstractions;
using HAGSJP.WeCasa.Models.Security;

namespace HAGSJP.WeCasa.Logging.Implementations
{
    /// <summary>
    /// Logger object used to record internal system events
    /// </summary>
    public class Logger : ILogger
    {
        private readonly ILoggerDAO _dao;
        public static string[] Categories = {"view", "business", "server", "data", "data store"};
        public static char[] specialCharacters = {',', '&', '?', '{', '}', '\\', '(', ')', '[', ']', '-', ';', '~', '|', '$', '!', '>', '*', '%', '_'};


        // Dependency inversion principle
        // Our logger is extensible
        public Logger(ILoggerDAO dao) // Inversion of control
        {
            _dao = dao;
        }

        /// <summary>
        /// Asynchronous method for building and sending logs to the database
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logLevel"></param>
        /// <param name="category"></param>
        /// <param name="rows"></param>
        /// <param name="username"></param>
        /// <returns>Result object</returns>
        public async Task<Result> Log(string message, LogLevel logLevel, string category, string username)
        {             
            var result = new Result();

            // Task Parallelism Library TPL
            // Getting number of processors
            //var numOfProcessors = Environment.ProcessorCount > 1 ? Environment.ProcessorCount - 1 : 1;

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

            // Step 2: Create a Log, Perform the logging
            // Database server(IP address w/ port), database, table, coloumn
            Log log = new Log(message, logLevel, category, username);

            var daoResult = await _dao.LogData(log).ConfigureAwait(false);
            Console.Write("DAOOO" + daoResult);

            if(daoResult.IsSuccessful)
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