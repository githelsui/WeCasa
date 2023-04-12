using System;
using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;
using HAGSJP.WeCasa.Services.Implementations;
using MySqlX.XDevAPI.CRUD;

namespace HAGSJP.WeCasa.Managers.Implementations
{
	public class ChoreManager
	{
        private readonly UserManager _um;
        private readonly ChoreService _service;
        private Logger _logger;

        public ChoreManager()
		{
            _logger = new Logger(new AccountMariaDAO());
            _service = new ChoreService();
        }

        public ChoreResult AddChore(Chore chore, UserAccount userAccount)
        {
            try
            {
                var result = new ChoreResult();

                chore.Created = DateTime.Now;
                chore.CreatedBy = userAccount.Username;

                // Populate AssignedTo property (if null -> AssignedTo = CreatedBy)
                if (chore.AssignedTo == null || chore.AssignedTo.Count == 0)
                {
                    List<String> assignedTo = new List<String>();
                    assignedTo.Add(chore.CreatedBy);
                    chore.AssignedTo = assignedTo;
                }

                // Validate all users in AssignedTo
                foreach (String username in chore.AssignedTo)
                {
                    // Check if user does not exist
                    if (!_um.IsUsernameTaken(username))
                    {
                        result.IsSuccessful = false;
                        result.Message = "Cannot assign chore to a user that does not exist.";
                        return result;
                    }
                }

                var serviceResult = _service.AddChore(chore);
                if (serviceResult.IsSuccessful)
                {
                    result.ReturnedObject = serviceResult.ReturnedObject;
                    _logger.Log("Add chore was successful", LogLevels.Info, "Data Store", userAccount.Username);
                }
                else
                {
                    _logger.Log( "Add chore error: " + result.ErrorStatus  + "\n" +"Message: " + result.Message, LogLevels.Error, "Service", userAccount.Username);
                }
                result.IsSuccessful = serviceResult.IsSuccessful;
                result.Message = serviceResult.Message;
                return result;
            }
            catch (Exception exc)
            {
                _logger.Log("Error Message: " + exc.Message, LogLevels.Error, "Service", userAccount.Username, new UserOperation(Operations.ChoreList, 0));
                throw exc;
            }
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

