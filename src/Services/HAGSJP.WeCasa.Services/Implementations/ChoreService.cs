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
                if(chore.Name.Length > 60) // Max chaacter length chore name
                {
                    result.IsSuccessful = false;
                    result.Message = "Chore name exceeds 60 character limit.";
                    return result;
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
            throw new NotImplementedException();
        }

        public ChoreResult GetGroupChores(GroupModel group)
        {
            throw new NotImplementedException();
        }

        public ChoreResult GetUserChores(UserAccount user)
        {
            throw new NotImplementedException();
        }

        public ChoreResult ValidateChore(Chore chore)
        {
            var result = new ChoreResult();

            // Input Validation
            if (chore.Name.Length > 60) // Max chaacter length chore name
            {
                result.IsSuccessful = false;
                result.Message = "Chore name exceeds 60 character limit.";
                return result;
            }

            var checkValidName = new Regex(@"\b([A-ZÀ-ÿ][-,a-z. ']*)+");
            if (!checkValidName.IsMatch(chore.Name)) // Chore name valid characters
            {
                result.IsSuccessful = false;
                result.Message = "Chore name has invalid characters.";
                return result;
            }

            if(chore.Notes.Length > 250) // Chore notes character limit
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

