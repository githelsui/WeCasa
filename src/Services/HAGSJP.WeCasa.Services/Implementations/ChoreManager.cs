using System;
using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;

namespace HAGSJP.WeCasa.Services.Implementations
{
	public class ChoreManager
	{
        private readonly ChoreService _service;
        private Logger _logger;

        public ChoreManager()
		{
            _logger = new Logger(new AccountMariaDAO());
            _service = new ChoreService();
        }

        public Result AddChore(Chore chore)
        {
            throw new NotImplementedException();
        }

        public Result EditChore(Chore chore)
        {
            throw new NotImplementedException();
        }

        public Result GetGroupToDoChores(GroupModel group)
        {
            throw new NotImplementedException();
        }

        public Result GetGroupCompletedChores(GroupModel group)
        {
            throw new NotImplementedException();
        }

        public Result GetUserToDoChores(UserAccount user)
        {
            throw new NotImplementedException();
        }

        public Result GetUserCompletedChores(UserAccount user)
        {
            throw new NotImplementedException();
        }
    }
}

