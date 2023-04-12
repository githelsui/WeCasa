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
                chore.IsCompleted = false;

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
                    _logger.Log("Add chore was successful", LogLevels.Info, "Service", userAccount.Username);
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

        public ChoreResult EditChore(Chore chore, UserAccount userAccount)
        {
            try
            {
                var result = new ChoreResult();

                chore.LastUpdated = DateTime.Now;
                chore.LastUpdatedBy = userAccount.Username;
                chore.IsCompleted = false;

                // Populate AssignedTo property (if null -> AssignedTo = CreatedBy)
                if (chore.AssignedTo == null || chore.AssignedTo.Count == 0)
                {
                    List<String> assignedTo = new List<String>();
                    assignedTo.Add(chore.CreatedBy);
                    chore.AssignedTo = assignedTo;
                }

                // Validate all users in new AssignedTo
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

                var serviceResult = _service.EditChore(chore);
                if (serviceResult.IsSuccessful)
                {
                    result.ReturnedObject = serviceResult.ReturnedObject;
                    _logger.Log("Edit chore was successful", LogLevels.Info, "Service", userAccount.Username);
                }
                else
                {
                    _logger.Log("Edit chore error: " + result.ErrorStatus + "\n" + "Message: " + result.Message, LogLevels.Error, "Service", userAccount.Username);
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

        public ChoreResult CompleteChore(Chore chore, UserAccount userAccount)
        {
            try
            {
                var result = new ChoreResult();

                chore.LastUpdated = DateTime.Now;
                chore.LastUpdatedBy = userAccount.Username;
                chore.IsCompleted = true;

                var serviceResult = _service.EditChore(chore);
                if (serviceResult.IsSuccessful)
                {
                    result.ReturnedObject = serviceResult.ReturnedObject;
                    _logger.Log("Chore completion saved successfully", LogLevels.Info, "Data Store", userAccount.Username);
                }
                else
                {
                    _logger.Log("Chore completion error: " + result.ErrorStatus + "\n" + "Message: " + result.Message, LogLevels.Error, "Service", userAccount.Username);
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

        public ChoreResult UndoChore(Chore chore, UserAccount userAccount)
        {
            try
            {
                var result = new ChoreResult();

                chore.LastUpdated = DateTime.Now;
                chore.LastUpdatedBy = userAccount.Username;
                chore.IsCompleted = false;

                var serviceResult = _service.EditChore(chore);
                if (serviceResult.IsSuccessful)
                {
                    result.ReturnedObject = serviceResult.ReturnedObject;
                    _logger.Log("Chore completion saved successfully", LogLevels.Info, "Service", userAccount.Username);
                }
                else
                {
                    _logger.Log("Chore completion error: " + result.ErrorStatus + "\n" + "Message: " + result.Message, LogLevels.Error, "Service", userAccount.Username);
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

        public ChoreResult GetGroupToDoChores(GroupModel group)
        {
            try
            {
                var result = new ChoreResult();

                var serviceResult = _service.GetGroupChores(group, 0);
                if (serviceResult.IsSuccessful)
                {
                    result.ReturnedObject = serviceResult.ReturnedObject;
                    _logger.Log("Group to-do chores fetched successfully", LogLevels.Info, "Service", group.Owner);
                }
                else
                {
                    _logger.Log("Chore fetch error: " + result.ErrorStatus + "\n" + "Message: " + result.Message, LogLevels.Error, "Service", group.Owner);
                }
                result.IsSuccessful = serviceResult.IsSuccessful;
                result.Message = serviceResult.Message;
                return result;
            }
            catch (Exception exc)
            {
                _logger.Log("Error Message: " + exc.Message, LogLevels.Error, "Service", group.Owner, new UserOperation(Operations.ChoreList, 0));
                throw exc;
            }
        }

        public ChoreResult GetGroupCompletedChores(GroupModel group)
        {
            try
            {
                var result = new ChoreResult();

                var serviceResult = _service.GetGroupChores(group, 1);
                if (serviceResult.IsSuccessful)
                {
                    result.ReturnedObject = serviceResult.ReturnedObject;
                    _logger.Log("Group completed chores fetched successfully", LogLevels.Info, "Service", group.Owner);
                }
                else
                {
                    _logger.Log("Chore fetch error: " + result.ErrorStatus + "\n" + "Message: " + result.Message, LogLevels.Error, "Service", group.Owner);
                }
                result.IsSuccessful = serviceResult.IsSuccessful;
                result.Message = serviceResult.Message;
                return result;
            }
            catch (Exception exc)
            {
                _logger.Log("Error Message: " + exc.Message, LogLevels.Error, "Service", group.Owner, new UserOperation(Operations.ChoreList, 0));
                throw exc;
            }
        }

        public ChoreResult GetUserToDoChores(UserAccount user)
        {
            try
            {
                var result = new ChoreResult();

                var serviceResult = _service.GetUserChores(user, 0);
                if (serviceResult.IsSuccessful)
                {
                    result.ReturnedObject = serviceResult.ReturnedObject;
                    _logger.Log("Group completed chores fetched successfully", LogLevels.Info, "Service", user.Username);
                }
                else
                {
                    _logger.Log("Chore fetch error: " + result.ErrorStatus + "\n" + "Message: " + result.Message, LogLevels.Error, "Service", user.Username);
                }
                result.IsSuccessful = serviceResult.IsSuccessful;
                result.Message = serviceResult.Message;
                return result;
            }
            catch (Exception exc)
            {
                _logger.Log("Error Message: " + exc.Message, LogLevels.Error, "Service", user.Username, new UserOperation(Operations.ChoreList, 0));
                throw exc;
            }
        }

        public ChoreResult GetUserCompletedChores(UserAccount user)
        {
            try
            {
                var result = new ChoreResult();

                var serviceResult = _service.GetUserChores(user, 1);
                if (serviceResult.IsSuccessful)
                {
                    result.ReturnedObject = serviceResult.ReturnedObject;
                    _logger.Log("User's completed chores fetched successfully", LogLevels.Info, "Service", user.Username);
                }
                else
                {
                    _logger.Log("Chore fetch error: " + result.ErrorStatus + "\n" + "Message: " + result.Message, LogLevels.Error, "Service", user.Username);
                }
                result.IsSuccessful = serviceResult.IsSuccessful;
                result.Message = serviceResult.Message;
                return result;
            }
            catch (Exception exc)
            {
                _logger.Log("Error Message: " + exc.Message, LogLevels.Error, "Service", user.Username, new UserOperation(Operations.ChoreList, 0));
                throw exc;
            }
        }
    }
}

