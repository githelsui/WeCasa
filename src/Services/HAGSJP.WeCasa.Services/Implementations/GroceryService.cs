using System;
using System.Text.RegularExpressions;
using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;
using MySqlConnector;
using MySqlX.XDevAPI.Common;

namespace HAGSJP.WeCasa.Services.Implementations
{
    public class GroceryService
    {
        private readonly GroceryDAO _dao;
        private Logger _logger;

        public GroceryService()
        {
            _logger = new Logger(new AccountMariaDAO());
            _dao = new GroceryDAO();
        }

        public GroceryResult AddGroceryItem(GroceryItem item)
        {
            try
            {
                var result = new GroceryResult();

                // Input Validation
                var validateResult = ValidateGroceryItem(item);
                if (!validateResult.IsSuccessful)
                {
                    return validateResult;
                }

                // DAO Operation
                var daoResult = _dao.AddGroceryItem(item);
                if (daoResult.IsSuccessful)
                {
                    result.ReturnedObject = daoResult.ReturnedObject;
                    _logger.Log("Grocery item created successfully", LogLevels.Info, "Data Store", item.CreatedBy);
                }
                else
                {
                    _logger.Log("Grocery item creation failed", LogLevels.Info, "Data Store", item.CreatedBy);
                }
                result.IsSuccessful = daoResult.IsSuccessful;
                result.Message = daoResult.Message;
                return result;
            }
            catch (Exception exc)
            {
                _logger.Log("Error Message: " + exc.Message, LogLevels.Error, "Data Store", item.CreatedBy, new UserOperation(Operations.GroceryList, 0));
                throw exc;
            }
        }

        public GroceryResult GetGroceryItems(GroupModel group)
        {
            try
            {
                var result = new GroceryResult();

                // DAO Operation
                var daoResult = _dao.GetGroceryItems(group);
                if (daoResult.IsSuccessful)
                {
                    result.ReturnedObject = daoResult.ReturnedObject;
                    _logger.Log("Grocery items fetched successfully from database", LogLevels.Info, "Data Store", group.Owner);
                }
                else
                {
                    _logger.Log("Failed to fetch grocery items from database", LogLevels.Info, "Data Store", group.Owner);
                }
                result.IsSuccessful = daoResult.IsSuccessful;
                result.Message = daoResult.Message;
                return result;
            }
            catch (Exception exc)
            {
                _logger.Log("Error Message: " + exc.Message, LogLevels.Error, "Data Store", group.Owner, new UserOperation(Operations.GroceryList, 0));
                throw exc;
            }
        }

        public GroceryResult ValidateGroceryItem(GroceryItem item)
        {
            var result = new GroceryResult();

            // Input Validation
            if (item.Name.Length > 60) // Max character length chore name
            {
                result.IsSuccessful = false;
                result.Message = "Grocery item name exceeds 60 character limit.";
                return result;
            }

            var checkValidName = new Regex(@"\b([A-ZÀ-ÿ][-,a-z. ']*)+");
            if (checkValidName.IsMatch(item.Name)) // grocery name invalid characters
            {
                result.IsSuccessful = false;
                result.Message = "Grocery item name has invalid characters.";
                return result;
            }

            if (item.Notes != null) // grocery notes character limit
            {
                if(item.Notes.Length > 250)
                {
                    result.IsSuccessful = false;
                    result.Message = "Grocery item notes exceeds 250 chracter limit.";
                    return result;
                }
            }
            result.IsSuccessful = true;
            return result;
        }
    }
    
}