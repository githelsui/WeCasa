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

        // Dependency inversion principle
        // Our logger is extensible
        public Logger(ILoggerDAO dao) // Inversion of control
        {
            _dao = dao;
        }

        public async Task<Result> Log(string message, string logLevel, string category, DateTime dateTime, int userId)
        {             
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

            if (message.Length > 200)
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Message log too long";

                return result;
            }
            // Invalid characters
            if (message.Contains("<"))
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Mesage contians < which is invalid";

                return result;
            }
            #endregion

            // Step 2: Perform the logging
            // Database server(IP address w/ port), database, table, coloumn
            var daoResult = await _dao.LogData(message, logLevel, category, dateTime, userId).ConfigureAwait(false);

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