using System;
using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Services.Implementations;
using HAGSJP.WeCasa.sqlDataAccess;

namespace HAGSJP.WeCasa.Services.Implementations
{
    public class NudgeService
    {
        private readonly NudgeDAO _dao;
        private Logger _logger;
        private Logger successLogger;
        private Logger errorLogger;

        public NudgeService()
        {
            _logger = new Logger(new AccountMariaDAO());
            _dao = new NudgeDAO();
            successLogger = new Logger(_dao);
            errorLogger = new Logger(_dao);
        }
        public DateTime? GetLastNudgeSent(int choreId, string senderEmail, string receiverEmail)
        {
            return _dao.GetLastNudgeSent(choreId, senderEmail, receiverEmail);
        }


        public DAOResult SendNudge(Nudge nudge) //or Result?
        {
            var result = _dao.SendNudge(nudge);
            return result;
        }
    }
}
