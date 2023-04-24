using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Managers.Abstractions;
using HAGSJP.WeCasa.sqlDataAccess;
using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.Services.Implementations;

namespace HAGSJP.WeCasa.Managers.Implementations
{
    public class FeedbackManager : IFeedbackManager
    {
            private readonly UserFeedbackDAO _dao;
            private Logger successLogger;
            private Logger errorLogger;
            private SendGridService sgs;

            public FeedbackManager()
            {
                _dao = new UserFeedbackDAO();
                successLogger = new Logger(_dao);
                errorLogger = new Logger(_dao);
                sgs = new SendGridService();
        }
            public FeedbackManager(UserFeedbackDAO dao)
            {
                _dao = dao;
                successLogger = new Logger(dao);
                errorLogger = new Logger(dao);
            }

            public Result storeFeedbackTicket(Feedback feedback)
            {
            try
            {
                DAOResult result = new DAOResult();
                result = _dao.storeFeedbackTicket(feedback);
                if (result.IsSuccessful)
                {
                    successLogger.Log("Storing feedback was successful", LogLevels.Info, "Data Store", feedback.Email);
                    sgs.SendFeedback();
                }
                else
                {
                    result.IsSuccessful = false;
                    errorLogger.Log("Storing feedback was unsuccessful", LogLevels.Error, "Data Store", feedback.Email);
                }
                return result;
            }
            catch (Exception ex) 
            {
                errorLogger.Log("Error Message: " + ex.Message, LogLevels.Error, "Data Store", feedback.Email);
                throw ex;
            }
        }
    }
}
