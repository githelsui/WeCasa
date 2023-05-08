using System;
using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.Services.Implementations;
using HAGSJP.WeCasa.sqlDataAccess;
using HAGSJP.WeCasa.Models;

namespace HAGSJP.WeCasa.Managers.Implementations
{
	public class AnalyticsManager
	{
        private readonly AnalyticsService _service;
        private readonly UserManager _userManager;
        private Logger _logger;

        public AnalyticsManager()
		{
            _service = new AnalyticsService();
            _userManager = new UserManager();
            _logger = new Logger(new AccountMariaDAO());
        }

        public KPIResult GetLoginsPerDay(UserAccount ua, String timeFrame)
        {
            try
            {
                var result = new KPIResult();

                var adminResult = _userManager.ValidateAdminRole(ua);
                if (!adminResult.IsSuccessful)
                {
                    result.Message = adminResult.Message;
                    result.IsSuccessful = false;
                    return result;
                }

                var isAdmin = (bool)adminResult.ReturnedObject;
                if (isAdmin)
                {
                    var minDate = GetMinDate(timeFrame);
                    var serviceResult = _service.GetLoginsPerDay(ua, minDate);
                    return serviceResult;
                }
                else
                {
                    result.Message += "User type does not have access to analytics.";
                    result.IsSuccessful = false;
                    return result;
                }
                return result;
            }
            catch (Exception exc)
            {
                _logger.Log("Error Message: " + exc.Message, LogLevels.Error, "Service", ua.Username);
                throw exc;
            }
        }

        public KPIResult GetRegistrationsPerDay(UserAccount ua, String timeFrame)
        {
            try
            {
                var result = new KPIResult();

                var adminResult = _userManager.ValidateAdminRole(ua);
                if (!adminResult.IsSuccessful)
                {
                    result.Message = adminResult.Message;
                    result.IsSuccessful = false;
                    return result;
                }

                var isAdmin = (bool)adminResult.ReturnedObject;
                if (isAdmin)
                {
                    var minDate = GetMinDate(timeFrame);
                    var serviceResult = _service.GetRegistrationsPerDay(ua, minDate);
                    return serviceResult;
                }
                else
                {
                    result.Message += "User type does not have access to analytics.";
                    result.IsSuccessful = false;
                    return result;
                }
                return result;
            }
            catch (Exception exc)
            {
                _logger.Log("Error Message: " + exc.Message, LogLevels.Error, "Service", ua.Username);
                throw exc;
            }
        }

        public KPIResult GetDailyActiveUsers(UserAccount ua, String timeFrame)
        {
            try
            {
                var result = new KPIResult();

                var adminResult = _userManager.ValidateAdminRole(ua);
                if (!adminResult.IsSuccessful)
                {
                    result.Message = adminResult.Message;
                    result.IsSuccessful = false;
                    return result;
                }

                var isAdmin = (bool)adminResult.ReturnedObject;
                if (isAdmin)
                {
                    var minDate = GetMinDate(timeFrame);
                    var serviceResult = _service.GetRegistrationsPerDay(ua, minDate);
                    return serviceResult;
                }
                else
                {
                    result.Message += "User type does not have access to analytics.";
                    result.IsSuccessful = false;
                    return result;
                }
                return result;
            }
            catch (Exception exc)
            {
                _logger.Log("Error Message: " + exc.Message, LogLevels.Error, "Service", ua.Username);
                throw exc;
            }
        }

        private DateTime GetMinDate(String timeFrame)
        {
            var minDate = new DateTime();
            var currentDate = DateTime.Now;
            switch (timeFrame)
            {
                case "1 Week":
                    minDate = currentDate.AddDays(-7).Date;
                    break;
                case "1 Month":
                    minDate = currentDate.AddMonths(-1).Date;
                    break;
                case "3 Months":
                    minDate = currentDate.AddMonths(-3).Date;
                    break;
                case "6 Months":
                    minDate = currentDate.AddMonths(-6).Date;
                    break;
                default:
                    minDate = currentDate.AddDays(-7).Date;
                    break;
            }
            return minDate;
        }
    }
}

