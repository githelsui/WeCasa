using System;
using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;
using MySqlConnector;

namespace HAGSJP.WeCasa.Services.Implementations
{
	public class ChoreService
	{
		private readonly ChoresDAO _dao;
		private Logger _logger;

		public ChoreService()
		{
            _logger = new Logger(new AccountMariaDAO());
            _dao = new ChoresDAO();
        }

		public Result AddChore(Chore chore)
		{
			throw new NotImplementedException();
		}

        public Result EditChore(Chore chore)
        {
            throw new NotImplementedException();
        }

        public Result GetGroupChores(GroupModel group)
        {
            throw new NotImplementedException();
        }

        public Result GetUserChores(UserAccount user)
        {
            throw new NotImplementedException();
        }
    }
}

