using System;
using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Services.Implementations;
using HAGSJP.WeCasa.sqlDataAccess;
using MySqlConnector;

namespace HAGSJP.WeCasa.Managers.Implementations
{
	public class NudgeManager
	{
        private Logger _logger;
        private readonly NudgeDAO _nudgeDAO;
        private readonly NudgeService _nudgeService;

        public NudgeManager()
		{
            _logger = new Logger(new AccountMariaDAO());
            //_nudgeDAO = new NudgeDAO();
            _nudgeService = new NudgeService();
        }

        public DateTime? GetLastNudgeSent(int choreId, string senderEmail, string recipientEmail)
        {
            return _nudgeService.GetLastNudgeSent(choreId, senderEmail, recipientEmail);
        }


        public Result SendNudge(Nudge nudge)
        {
            //var result = _nudgeDAO.SendNudge(nudge); //dao or service?
            var result = _nudgeService.SendNudge(nudge);
            return result;
        }
    }
}

