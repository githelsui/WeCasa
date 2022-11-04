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

        // dependency inversion principle
        // our logger is extensible
        public Logger(ILoggerDAO dao) // inversion of control
        {
            _dao = dao;
        }

        public Result Log(String message)
        {
            // TODO: Log to database
            var result = new Result();

            // Step 1: Input Validation (include bool AND feedback for what went wrong)
            #region Step 1: Input validation
            if (message == null)
            {
                result.IsSuccessful = ResultStatus.Success;
                return result;
            }

            if (message.Length > 200)
            {
                result.IsSuccessful = ResultStatus.Faulty;
                result.ErrorMessage = "Message log too long";
                return result;
            }
            // invalid characters
            if (message.Contains("<"))
            {
                result.IsSuccessful = ResultStatus.Faulty;
                result.ErrorMessage = "Mesage contians < which is invalid";
                return result;
            }
            #endregion

            // Step 2: Perform the logging
            // database server(IP address w/ port), database, table, coloumn
            var daoResult = _dao.LogData(message);

            if(daoResult.IsSuccessful)
            {
                result.IsSuccessful = true;

                return result;
            }

            // translate the error to something that is more user friendly or layer friendly

            result.IsSuccessful = false;
            result.ErrorMessage = daoResult.ErrorMessage;

            return result;
        }
    }
}