using System;
using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;

namespace HAGSJP.WeCasa.Services.Implementations
{
	public class ChoreManager
	{
        private readonly UserManager _dao;
        private readonly ChoreService _service;
        private Logger _logger;

        public ChoreManager()
		{
            _logger = new Logger(new AccountMariaDAO());
            _service = new ChoreService();
        }

        public ChoreResult AddChore(Chore chore)
        {
            //TODO: New parameter for UserAccount user making this operation
            // Create Chore object here from parameters
            // -> Populate Created property
            // -> Populate CreatedBy property
            // -> Populate AssignedTo property (if null -> AssignedTo = CreatedBy)
            // ---> Use UserManager to validate all usernames in AssignedTo
            //if (chore.AssignedTo.Count > 0)
            //{
            //    foreach (String username in chore.AssignedTo)
            //    {
            //        // Check if user exists
            //    }
            //}

            // Business logic
            // --> Calls service layer function

            throw new NotImplementedException();
        }

        public ChoreResult EditChore(Chore chore)
        {
            throw new NotImplementedException();
        }

        public ChoreResult GetGroupToDoChores(GroupModel group)
        {
            throw new NotImplementedException();
        }

        public ChoreResult GetGroupCompletedChores(GroupModel group)
        {
            throw new NotImplementedException();
        }

        public ChoreResult GetUserToDoChores(UserAccount user)
        {
            throw new NotImplementedException();
        }

        public ChoreResult GetUserCompletedChores(UserAccount user)
        {
            throw new NotImplementedException();
        }
    }
}

