using System;
using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.sqlDataAccess;
using HAGSJP.WeCasa.Models;
using System.Collections.Generic;
using Org.BouncyCastle.Utilities;
using System.Linq;

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
                    _logger.Log("Get logins per day failed", LogLevels.Error, "Data Store", ua.Username);
                    result.IsSuccessful = false;
                    result.Message = daoResult.Message;
                }
                else
                {
                    _logger.Log("Get logins per day success", LogLevels.Info, "Data Store", ua.Username);
                    var loginsPerDay = (List<Dictionary<String, String>>)daoResult.ReturnedObject;
                    var minDateStr = minDate.ToShortDateString();
                    var maxDateStr = DateTime.Now.Date.ToShortDateString();
                    var addMinDate = true;
                    var addMaxDate = true;
                    foreach (Dictionary <String,String> loginData in loginsPerDay)
                    {
                        var date = loginData["date"];
                        if(date == minDateStr)
                        {
                            addMinDate = false;
                        }

                        if(date == maxDateStr)
                        {
                            addMaxDate = false;
                        }
                    }

                    if (addMinDate)
                    {
                        var minDateValue = new Dictionary<String, String>(){
                        { "date", minDate.ToShortDateString() },
                        { "value", "0" }
                        };
                        loginsPerDay.Insert(0, minDateValue);
                    }

                    if (addMaxDate)
                    {
                        var maxDateValue = new Dictionary<String, String>(){
                        { "date", DateTime.Now.Date.ToShortDateString() },
                        { "value", "0" }
                         };
                        loginsPerDay.Add(maxDateValue);
                    }

                    result.ReturnedObject = loginsPerDay;
                    result.IsSuccessful = true;
                    result.Message = daoResult.Message;
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

