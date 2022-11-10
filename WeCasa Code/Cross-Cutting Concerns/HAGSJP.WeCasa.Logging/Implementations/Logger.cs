using Microsoft.VisualBasic;
using Microsoft.Data.SqlClient;
using HAGSJP.WeCasa.sqlDataAccess;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Logging.Abstractions;
using HAGSJP.WeCasa.sqlDataAccess.Abstractions;

namespace HAGSJP.WeCasa.Logging.Implementations
{

    public class Logger : ILogger
    {
        private readonly ILoggerDAO _dao;
        public static string[] LogLevels = {"Info", "Debug", "Warning", "Error"};
        public static string[] Categories = {"View", "Business", "Server", "Data", "Data Store"};
        public static char[] specialCharacters = {',', '&', '?', '{', '}', '\\', '(', ')', '[', ']', '-', ';', '~', '|', '$', '!', '>', '*', '%', '_'};


        // Dependency inversion principle
        // Our logger is extensible
        public Logger(ILoggerDAO dao) // Inversion of control
        {
            _dao = dao;
        }

        public async Task<Result> Log(string message, string logLevel, string category, string username)
        {     
            // System.Timers.Timer aTimer = new System.Timers.Timer();
            // aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            // aTimer.Close = 5000;
            // aTimer.Enabled = true; 
            var result = new Result();

            // Task Parallelism Library TPL
            // Getting number of processors
            //var numOfProcessors = Environment.ProcessorCount > 1 ? Environment.ProcessorCount - 1 : 1;

            // Step 1: Input Validation (include bool AND feedback for what went wrong)
            #region Step 1: Input validation
            if (message == null)
            {
                result.IsSuccessful = false;

                return result;
            }
            // Invalid length
            if (message.Length > 200)
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Message log too long";

                return result;
            }
            // Invalid characters
            var matchedIndex = message.IndexOfAny(specialCharacters);
            if (matchedIndex != -1)
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Message contains invalid character: " + message[matchedIndex];

                return result;
            }
            // Invalid Log Level
            if (!LogLevels.Contains(logLevel))
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Invalid log level";

                return result;
            }
            // Invalid Category
            if (!Categories.Contains(category))
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Invalid category";

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

            // Translate the error to something that is more user friendly or layer friendly

            result.IsSuccessful = false;
            result.ErrorMessage = daoResult.ErrorMessage;

            return result;
        }
    }
}