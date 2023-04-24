using System;
using System.Text.RegularExpressions;
using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;
using MySqlConnector;
using MySqlX.XDevAPI.Common;

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

		public ChoreResult AddChore(Chore chore)
		{
            try
            {
                var result = new ChoreResult();

                // Input Validation
                var validateResult = ValidateChore(chore);
                if (!validateResult.IsSuccessful) // Max chaacter length chore name
                {
                    return validateResult;
                }

                // DAO Operation
                var daoResult = _dao.CreateChore(chore);
                if (daoResult.IsSuccessful)
                {
                    result.ReturnedObject = daoResult.ReturnedObject;
                    _logger.Log("Chore created successfully", LogLevels.Info, "Data Store", chore.CreatedBy);
                }
                else
                {
                    _logger.Log("Chore creation failed", LogLevels.Info, "Data Store", chore.CreatedBy);
                }
                result.IsSuccessful = daoResult.IsSuccessful;
                result.Message = daoResult.Message;
                return result;
            }
            catch(Exception exc)
            {
                _logger.Log("Error Message: " + exc.Message, LogLevels.Error, "Data Store", chore.CreatedBy, new UserOperation(Operations.ChoreList, 0));
                throw exc;
            }
           
		}

        public ChoreResult EditChore(Chore chore)
        {
            try
            {
                var result = new ChoreResult();

                // Input Validation
                var validateResult = ValidateChore(chore);
                if (!validateResult.IsSuccessful) // Max chaacter length chore name
                {
                    return validateResult;
                }

                // DAO Operation
                var daoResult = _dao.UpdateChore(chore);
                if (daoResult.IsSuccessful)
                {
                    result.ReturnedObject = daoResult.ReturnedObject;
                    _logger.Log("Chore edited successfully", LogLevels.Info, "Data Store", chore.LastUpdatedBy);
                }
                else
                {
                    _logger.Log("Chore edit failed", LogLevels.Info, "Data Store", chore.LastUpdatedBy);
                }
                result.IsSuccessful = daoResult.IsSuccessful;
                result.Message = daoResult.Message;
                return result;
            }
            catch (Exception exc)
            {
                _logger.Log("Error Message: " + exc.Message, LogLevels.Error, "Data Store", chore.CreatedBy, new UserOperation(Operations.ChoreList, 0));
                throw exc;
            }
        }

        public ChoreResult CompleteChore(Chore chore)
        {
            try
            {
                var result = new ChoreResult();
                // DAO Operation
                var daoResult = _dao.CompleteChore(chore);
                if (daoResult.IsSuccessful)
                {
                    result.ReturnedObject = daoResult.ReturnedObject;
                    _logger.Log("Chore completed successfully", LogLevels.Info, "Data Store", chore.LastUpdatedBy);
                }
                else
                {
                    _logger.Log("Chore completion failed", LogLevels.Info, "Data Store", chore.LastUpdatedBy);
                }
                result.IsSuccessful = daoResult.IsSuccessful;
                result.Message = daoResult.Message;
                return result;
            }
            catch (Exception exc)
            {
                _logger.Log("Error Message: " + exc.Message, LogLevels.Error, "Data Store", chore.CreatedBy, new UserOperation(Operations.ChoreList, 0));
                throw exc;
            }
        }

        public ChoreResult DeleteChore(Chore chore)
        {
            try
            {
                var result = new ChoreResult();
                // DAO Operation
                var daoResult = _dao.DeleteChore(chore);
                if (daoResult.IsSuccessful)
                {
                    result.ReturnedObject = daoResult.ReturnedObject;
                    _logger.Log("Chore deleted successfully", LogLevels.Info, "Data Store", chore.LastUpdatedBy);
                }
                else
                {
                    _logger.Log("Chore deletion failed", LogLevels.Info, "Data Store", chore.LastUpdatedBy);
                }
                result.IsSuccessful = daoResult.IsSuccessful;
                result.Message = daoResult.Message;
                return result;
            }
            catch (Exception exc)
            {
                _logger.Log("Error Message: " + exc.Message, LogLevels.Error, "Data Store", chore.CreatedBy, new UserOperation(Operations.ChoreList, 0));
                throw exc;
            }
        }

        public ChoreResult GetGroupChores(GroupModel group, int isCompleted)
        {
            try
            {
                var result = new ChoreResult();

                // DAO Operation
                var selectSql = "";
                if (isCompleted == 1)
                {
                    selectSql = string.Format(@"SELECT * from CHORES WHERE group_id = '{0}' AND is_completed = '{1}' ORDER BY last_updated DESC ", group.GroupId, isCompleted);
                }
                else
                {
                    selectSql = string.Format(@"SELECT * from CHORES WHERE group_id = '{0}'", group.GroupId);
                }

                var daoResult = _dao.GetChores(selectSql);
                if (daoResult.IsSuccessful)
                {
                    result.ReturnedObject = daoResult.ReturnedObject;
                    _logger.Log("Group chores fetched successfully", LogLevels.Info, "Data Store", group.Owner);
                }
                else
                {
                    _logger.Log("Group chores fetch failed", LogLevels.Info, "Data Store", group.Owner);
                }
                result.IsSuccessful = daoResult.IsSuccessful;
                result.Message = daoResult.Message;
                return result;
            }
            catch (Exception exc)
            {
                _logger.Log("Error Message: " + exc.Message, LogLevels.Error, "Data Store", group.Owner, new UserOperation(Operations.ChoreList, 0));
                throw exc;
            }
        }

        public ChoreResult GetUserChores(UserAccount user, int isCompleted)
        {
            try
            {
                var result = new ChoreResult();

                // DAO Operations
                // Preparation of first sql query from UserChore table
                var selectSql = string.Format(@"SELECT * from USERCHORE WHERE username = '{0}' AND is_completed = '{1}'", user.Username, isCompleted);
                var daoResult = _dao.GetUserChores(selectSql);
                
                if (daoResult.IsSuccessful)
                {
                    List<int> choreIds = (List<int>)daoResult.ReturnedObject;
                    if(choreIds.Count > 0)
                    {
                        // Preparation of second sql query from Chores table
                        string choreIdsStr = string.Join(",", choreIds);
                        var selectChoreSql = string.Format(@"SELECT * from CHORES WHERE chore_id IN ('{0}')", choreIdsStr);
                        daoResult = _dao.GetChores(selectChoreSql);
                        if (daoResult.IsSuccessful)
                        {
                            result.ReturnedObject = daoResult.ReturnedObject;
                            _logger.Log("User chores fetched successfully", LogLevels.Info, "Data Store", user.Username);
                        }
                        else
                        {
                            _logger.Log("User chores fetched failed from Chores", LogLevels.Info, "Data Store", user.Username);
                        }
                    }
                }
                else
                {
                    _logger.Log("User chores fetched failed from UserChore", LogLevels.Info, "Data Store", user.Username);
                }
                result.IsSuccessful = daoResult.IsSuccessful;
                result.Message = daoResult.Message;
                return result;
            }
            catch (Exception exc)
            {
                _logger.Log("Error Message: " + exc.Message, LogLevels.Error, "Data Store", user.Username, new UserOperation(Operations.ChoreList, 0));
                throw exc;
            }
        }

        public ChoreResult ValidateChore(Chore chore)
        {
            var result = new ChoreResult();

            // Input Validation
            if (chore.Name.Length > 60) // Max character length chore name
            {
                result.IsSuccessful = false;
                result.Message = "Chore name exceeds 60 character limit.";
                return result;
            }

            var checkValidName = new Regex(@"[^a-zA-Z0-9\s]");
            if (checkValidName.IsMatch(chore.Name)) // Chore name invalid characters
            {
                result.IsSuccessful = false;
                result.Message = "Chore name has invalid characters.";
                return result;
            }

            if(chore.Notes != null && chore.Notes.Length > 250) // Chore notes character limit
            {
                result.IsSuccessful = false;
                result.Message = "Chore notes exceeds 250 chracter limit.";
                return result;
            }

            DateTime currentDt = DateTime.Now;
            if (chore.ResetTime.HasValue && DateTime.Compare((DateTime)chore.ResetTime, currentDt) < 0) // Reset time validation
            {
                // Chore reset time is earlier than current date
                result.IsSuccessful = false;
                result.Message = "Chore reset time cannot be due before current date.";
                return result;
            }

            result.IsSuccessful = true;
            return result;
        }
    }
}

