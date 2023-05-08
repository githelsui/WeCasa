using System;
using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.sqlDataAccess;
using HAGSJP.WeCasa.Models;

namespace HAGSJP.WeCasa.Services.Implementations
{
	public class AnalyticsService
	{
        private readonly Logger _logger;
        private readonly AnalyticsDAO _dao;

        public AnalyticsService()
		{
            _logger = new Logger(new AccountMariaDAO());
            _dao = new AnalyticsDAO();
        }

        public KPIResult GetLoginsPerDay(UserAccount ua, DateTime minDate)
        {
            try
            {
                var result = new KPIResult();

                var daoResult = _dao.GetLoginsPerDay(minDate);
                if (!daoResult.IsSuccessful)
                {
                    _logger.Log("Get logins per day failed", LogLevels.Info, "Data Store", ua.Username);
                    result.IsSuccessful = false;
                    result.Message = daoResult.Message;
                    return result;
                }
                else
                {
                    //var loginsPerDate = new List<Dictionary<DateTime, int>>();
                    //var logsList = (List<Log>)daoResult.ReturnedObject;
                    //// Parse through the data as an enumerable
                    //foreach(Log log in logsList)
                    //{
                    //    //var metric = { log.Date_Time, log.}
                    //}
                }
                
                return result;
            }
            catch (Exception exc)
            {
                _logger.Log("Error Message: " + exc.Message, LogLevels.Error, "Data Store", ua.Username);
                throw exc;
            }
        }
    }
}

