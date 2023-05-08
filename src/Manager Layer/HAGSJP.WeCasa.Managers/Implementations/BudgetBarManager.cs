using System.Text.RegularExpressions;
using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Services.Implementations;
using HAGSJP.WeCasa.sqlDataAccess;
using MySqlConnector;

namespace HAGSJP.WeCasa.Managers.Implementations
{
    public class BudgetBarManager
    {
        private readonly BudgetBarDAO _dao;
        private Logger _logger;
        private RemindersDAO remindersDAO;


        public BudgetBarManager() 
        {
            _logger = new Logger(new AccountMariaDAO());
            _dao = new BudgetBarDAO();
            remindersDAO = new RemindersDAO();

        }

        public BudgetBarView GetInitialBudgetBarVew(int groupId)
        {
            try
            {
                // Check if GroupId is valid
                Boolean validId = ValidateId(groupId); 
                if (!validId)
                {
                    throw new Exception("Invalid GroupId");
                }
                Logger lo = new Logger(new GroupMariaDAO());
                AccountMariaDAO dao = new AccountMariaDAO();
                decimal budget = GetBudget(groupId);
                Dictionary<string, string> names = dao.GetFirstNames(groupId);
                List<BudgetBarUser> budgetBarUsers = new List<BudgetBarUser>();
                Dictionary<string, decimal> userTotals = new Dictionary<string, decimal>();
                Decimal totalSpent = 0;

                // Get first name for all members
                foreach(var name in names)
                {
                    BudgetBarUser bbUser = new BudgetBarUser(name.Key, name.Value);
                    budgetBarUsers.Add(bbUser);
                    userTotals.Add(name.Key, 0);
                }

                // Get bills
                List<Bill> bills = GetBills(groupId);
                List<Bill> activeBills = new List<Bill>();
                List<Bill> deletedBills = new List<Bill>();
                // sort bills into active and deleted bills

                foreach(Bill bill in bills)
                {
                    if (bill.PaymentStatus==true || bill.IsDeleted==true)
                    {
                        deletedBills.Add(bill);
                    }
                    else
                    {
                        activeBills.Add(bill);
                        // Calculate total spent in the group
                        totalSpent += bill.Amount;
                        // Calculate individual totals
                        foreach(var user in bill.Usernames) {
                            userTotals[user] += bill.Amount/(bill.Usernames.Count);
                        }
                    }
                }

                BudgetBarView bbView = new BudgetBarView();
                bbView.ActiveBills = activeBills;
                bbView.DeletedBills = deletedBills;
                bbView.Group = budgetBarUsers;
                bbView.Budget = budget;
                bbView.GroupTotal = totalSpent;
                bbView.UserTotals = userTotals;
                
                return bbView;
            } 
            catch (Exception exc)
            {
                _logger.Log( "Error Message: " + exc.Message, LogLevels.Error, "Data Store", "Group ID: " + groupId, new UserOperation(Operations.BudgetBar,0));
                throw exc;
            }
        }

        public Result InsertBill(Bill bill)
        {
            try
            {
                // validate members
                if (bill.Usernames.Count == 0)
                {
                    bill.Usernames.Add(bill.Owner);
                }
                // Validate bill
                bool validBill = ValidateBill(bill);
                DAOResult result = new DAOResult();
                if (validBill)
                {
                    // Insert Bill
                    result = _dao.InsertBill(bill);
                    if (result.IsSuccessful)
                    {
                        Console.WriteLine("insert bill was successful");
                        var group = new GroupModel { GroupId = bill.GroupId };
                        var emails = remindersDAO.GetGroupEmail(group);
                        var usernames = (List<string>)emails.ReturnedObject;
                        var from = "wecasacsulb@gmail.com";
                        var subject = "New bill shared with you";
                        var message = "New bill has been share with you in WeCasa";
                        var rem = "immediately";
                        var evnt = "New bill added to budget";
                        List<string> recipient = new List<string>();
                        for (int i = 0; i < usernames.Count; i++)
                        {
                            
                                Console.WriteLine(usernames.Count);
                                var to = usernames[i];
                                Console.WriteLine("Sending to: "+ to);
                                NotificationService.ScheduleReminderEmail(from, to, subject, message, rem, evnt);
                                Console.WriteLine("Sent: " + to);

                        }
                        _logger.Log("Add bill was successful " + bill.Amount, LogLevels.Info, "Data Store", bill.Owner);
                    }
                    else
                    {
                        _logger.Log("Add bill Error: " + result.ErrorStatus + "\n" + "Message: " + result.Message + "\n" + "State: " + result.SqlState, LogLevels.Error, "Data Store", bill.Owner);
                    }
                }
                else
                {
                    result.IsSuccessful = false;
                    result.Message = "Invalid Bill";
                }
                return result;
            }
            catch (Exception exc)
            {
                _logger.Log("Error Message: " + exc.Message, LogLevels.Error, "Data Store", bill.Owner, new UserOperation(Operations.BudgetBar, 0));
                throw exc;
            }
        }

        public Result UpdateBill(Bill bill)
        {
            try
            {
                // validate members 
                if (bill.Usernames.Count == 0) 
                {
                    bill.Usernames.Add(bill.Owner);
                }
                bool validBill = ValidateBill(bill);
                DAOResult result = new DAOResult();
                if (validBill)
                {
                    result = _dao.UpdateBill(bill);
                    if (result.IsSuccessful)
                    {
                        _logger.Log("Edit bill was successful", LogLevels.Info, "Data Store", bill.Owner);
                    }
                    else
                    {
                        _logger.Log( "Edit bill Error: " + result.ErrorStatus  + "\n" +"Message: " + result.Message + "\n" + "State: " + result.SqlState, LogLevels.Error, "Data Store", bill.Owner);
                    }
                }
                else 
                {
                    result.IsSuccessful = false;
                    result.Message = "Invalid Bill";
                }
                return result;
            }
            catch (Exception exc)
            {
                _logger.Log( "Error Message: " + exc.Message, LogLevels.Error, "Data Store", bill.Owner, new UserOperation(Operations.BudgetBar,0));
                throw exc;
            }
        }

        public decimal GetBudget(int groupId)
        {
            try
            {
                decimal result = _dao.GetBudget(groupId);
                if (result < 0) {
                    _logger.Log( "Get Budget Failed", LogLevels.Error, "Data Store", "Group ID: " + groupId);
                }
                return result;
            }
             catch(MySqlException exc)
            {
                _logger.Log( "Error: " + exc.ErrorCode  + "\n" +"Message: " + exc.Message + "\n" + "State: " + exc.SqlState, LogLevels.Error, "Data Store", "Group ID: " + groupId);
                throw exc;
            }
        }

        public Result EditBudget(int groupId, decimal amount)
        {
            try 
            {
                // Check if GroupId is valid
                Boolean validId = ValidateId(groupId); 
                if (!(amount >= 0 && validId))
                {
                    Result InvalidResult = new Result();
                    InvalidResult.IsSuccessful = false;
                    InvalidResult.Message = "Invalid GroupId";
                    return InvalidResult;
                }
                // Edit Budget
                DAOResult result = _dao.EditBudget(groupId, amount);
                if (result.IsSuccessful)
                {
                    _logger.Log("Edit budget was successful", LogLevels.Info, "Data Store", "Group Id" + groupId);
                }
                else
                {
                    _logger.Log( "Edit budget Error: " + result.ErrorStatus  + "\n" +"Message: " + result.Message + "\n" + "State: " + result.SqlState, LogLevels.Error, "Data Store", "Group ID: " + groupId);
                }
                return result;
            }
            catch (Exception exc)
            {
                _logger.Log( "Error Message: " + exc.Message, LogLevels.Error, "Data Store", "GroupID: " + groupId, new UserOperation(Operations.BudgetBar,0));
                throw exc;
            }
        }

        public Result DeleteBill(int billId)
        {
            try
            {
                // Check if GroupId is valid
                Boolean validId = ValidateId(billId); 
                if (!validId)
                {
                    Result InvalidResult = new Result();
                    InvalidResult.IsSuccessful = false;
                    InvalidResult.Message = "Invalid BillId";
                    return InvalidResult;
                }
                // Delete bill
                DAOResult result = _dao.DeleteBill(billId, DateTime.Now);
                if (result.IsSuccessful)
                {
                    _logger.Log("Delete bill was successful", LogLevels.Info, "Data Store", "Bill Id: " + billId);
                }
                else
                {
                    _logger.Log( "Delete bill Error: " + result.ErrorStatus  + "\n" +"Message: " + result.Message + "\n" + "State: " + result.SqlState, LogLevels.Error, "Data Store", "Bill Id: " + billId);
                }
                return result;      
            }
            catch (Exception exc)
            {
                _logger.Log( "Error Message: " + exc.Message, LogLevels.Error, "Data Store", "Bill Id: " + billId, new UserOperation(Operations.BudgetBar,0));
                throw exc;
            }
        }

        public Result RestoreDeletedBill(int billId)
        {
            try
            {
                // Check if BillId is valid
                Boolean validId = ValidateId(billId); 
                if (!validId)
                {
                    Result InvalidResult = new Result();
                    InvalidResult.IsSuccessful = false;
                    InvalidResult.Message = "Invalid BillId";
                    return InvalidResult;
                }
                // Restore bill
                DAOResult result = _dao.RestoreDeletedBill(billId);
                if (result.IsSuccessful)
                {
                    _logger.Log("Restore bill was successful", LogLevels.Info, "Data Store", "Bill Id: " + billId);
                }
                else
                {
                    _logger.Log( "Restore bill Error: " + result.ErrorStatus  + "\n" +"Message: " + result.Message + "\n" + "State: " + result.SqlState, LogLevels.Error, "Data Store", "Bill Id: " + billId);
                }
                return result;
            }
            catch (Exception exc)
            {
                _logger.Log( "Error Message: " + exc.Message, LogLevels.Error, "Data Store", "Bill Id: " + billId, new UserOperation(Operations.BudgetBar,0));
                throw exc;
            }
        }

        public List<Bill> GetBills(int groupId)
        {
            try
            {
                // Check if GroupId is valid
                Boolean validId = ValidateId(groupId); 
                if (!validId)
                {
                    throw new Exception("Invalid GroupId");
                }
                // Get bills
                List<Bill> bills = _dao.GetBills(groupId);
                _logger.Log( "Bill" + bills, LogLevels.Error, "Data Store", "Group ID: " + groupId);

                return bills;
            }
            catch(MySqlException exc)
            {
                _logger.Log( "Error: " + exc.ErrorCode  + "\n" +"Message: " + exc.Message + "\n" + "State: " + exc.SqlState, LogLevels.Error, "Data Store", "Group ID: " + groupId);
                throw exc;
            }
        }

        public Boolean ValidateBill(Bill bill)
        {

            // validate name
            if (!Regex.IsMatch(bill.BillName, "^[a-zA-Z0-9 ]{1,60}$"))
            {
                _logger.Log("Validate Bill: Invalid Name", LogLevels.Error, "Data", bill.Owner);
                return false;
            }
            // validate description
            if (bill.BillDescription != null  && !Regex.IsMatch(bill.BillDescription, "^[a-zA-Z0-9 ]{0,2000}$"))
            {
                _logger.Log("Validate Bill: Invalid Description", LogLevels.Error, "Data", bill.Owner);
                return false;
            }
            // validate amount
            if (!(bill.Amount >= 0 && Regex.IsMatch(bill.Amount.ToString(), @"^\d+(\.\d{1,2})?$"))) 
            {
                _logger.Log("Validate Bill: Invalid amount " + bill.Amount , LogLevels.Error, "Data", bill.Owner);
                return false;
            }
            // validate file extension of receipt
            string fileExtension = bill.PhotoFileName!=null ? Path.GetExtension(bill.PhotoFileName) : "";
            if (fileExtension.Length != 0 && fileExtension != ".png" && fileExtension != ".jpeg" && fileExtension != ".heif")
            {
                _logger.Log("Validate Bill: Invalid file", LogLevels.Error, "Data", bill.Owner);
                return false;
            }
            // TODO: validate optional fields exist
            return true;
        }

        public Boolean ValidateId(int Id) 
        {
            if (Id < 0)
            {
                return false;
            }
            return true;
        }
    }
}