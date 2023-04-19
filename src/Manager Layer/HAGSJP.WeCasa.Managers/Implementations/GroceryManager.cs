using System;
using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;
using HAGSJP.WeCasa.Services.Implementations;
using MySqlX.XDevAPI.CRUD;
using MySqlX.XDevAPI.Common;

namespace HAGSJP.WeCasa.Managers.Implementations
{
    public class GroceryManager
    {
        private readonly UserManager _um;
        private readonly GroceryService _service;
        private Logger _logger;

        public GroceryManager()
        {
            _logger = new Logger(new AccountMariaDAO());
            _um = new UserManager();
            _service = new GroceryService();
        }

        public GroceryResult AddGroceryItem(GroceryItem item, UserAccount userAccount)
        {
            try
            {
                var result = new GroceryResult();

                item.Created = DateTime.Now;
                item.CreatedBy = userAccount.Username;
                item.IsPurchased = false;

                var assignedProfilesRes = AssignItem(item);
                if (assignedProfilesRes.IsSuccessful)
                {
                    item.Assignments = (List<string>)assignedProfilesRes.ReturnedObject;
                }
                else
                {
                    result.IsSuccessful = false;
                    result.Message = assignedProfilesRes.Message;
                    return result;
                }

                var serviceResult = _service.AddGroceryItem(item);
                if (serviceResult.IsSuccessful)
                {
                    result.ReturnedObject = serviceResult.ReturnedObject;
                    _logger.Log("Add Grocery was successful", LogLevels.Info, "Service", userAccount.Username);
                }
                else
                {
                    _logger.Log("Add Grocery error: " + result.ErrorStatus + "\n" + "Message: " + result.Message, LogLevels.Error, "Service", userAccount.Username);
                }
                result.IsSuccessful = true;
                result.Message = serviceResult.Message;
                return result;
            }
            catch (Exception exc)
            {
                _logger.Log("Error Message: " + exc.Message, LogLevels.Error, "Service", userAccount.Username, new UserOperation(Operations.GroceryList, 0));
                throw exc;
            }
        }

        //Assignment Validation
        private GroceryResult AssignItem(GroceryItem item)
        {
            var result = new GroceryResult();

            Console.WriteLine(string.Join(", ", item.Assignments));

            // If no assignments were manually set, assign to creator
            if (item.Assignments == null || item.Assignments.Count == 0)
            {
                Console.WriteLine("item.Assignments is not null");
                List<String> assignedToStr = new List<String>();
                assignedToStr.Add(item.CreatedBy);
                Console.WriteLine("created by: " + item.CreatedBy);
                item.Assignments = assignedToStr;
            }

            // Validate all users
            foreach (string username in item.Assignments)
            {
                //Check if user does not exist
                if (!_um.IsUsernameTaken(username))
                {
                    result.IsSuccessful = false;
                    result.Message = "Cannot assign grocery item to a user that does not exist.";
                    return result;
                }
            }
            result.IsSuccessful = true;
            result.Message = "Successfully validated all assigned user's profiles.";
            result.ReturnedObject = item.Assignments;
            return result;
        }
    }
}