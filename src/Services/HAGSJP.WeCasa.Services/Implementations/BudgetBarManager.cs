using System.Net;
using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;
using MySqlConnector;

namespace HAGSJP.WeCasa.Services.Implementations
{
    public class BudgetBarManager
    {
        private readonly BudgetBarDAO _dao;
        private Logger _logger;

        public  BudgetBarManager() 
        {
            _logger = new Logger(new AccountMariaDAO());
            _dao = new BudgetBarDAO();
        }
        public string generateID()
        {
            return Guid.NewGuid().ToString("N");
        }

        public BudgetBarView GetInitialBudgetBarVew(int groupId)
        {
            Logger lo = new Logger(new GroupMariaDAO());
            AccountMariaDAO dao = new AccountMariaDAO();
            // RefreshBillList(username);
            // DeleteAllOutdatedBills(username);
            decimal budget = GetBudget(groupId);
            Dictionary<string, string> names = dao.GetFirstNames(groupId);
            List<BudgetBarUser> budgetBarUsers = new List<BudgetBarUser>();
            Dictionary<string, decimal> userTotals = new Dictionary<string, decimal>();
            Decimal totalSpent = 0;
            foreach(var name in names)
            {
                BudgetBarUser bbUser = new BudgetBarUser(name.Key, name.Value);
                budgetBarUsers.Add(bbUser);
                userTotals.Add(name.Key, 0);
            }
            List<Bill> bills = GetBills(groupId);
            List<Bill> activeBills = new List<Bill>();
            List<Bill> deletedBills = new List<Bill>();
            foreach(Bill bill in bills)
            {
                // TODO: to bill.dateDeleted != null
                if (bill.IsDeleted == false)
                {
                    activeBills.Add(bill);
                    totalSpent += bill.Amount;
                    foreach(var user in bill.Usernames) {
                        userTotals[user] += bill.Amount/(bill.Usernames.Count);
                    }
                }
                else 
                {
                    deletedBills.Add(bill);
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
 
        public Result DeleteAllOutdatedBills(string username)
        {
            Result deleteOldBillsResult = _dao.DeleteAllOutdatedBills();
            if (!deleteOldBillsResult.IsSuccessful)
            {
                _logger.Log(deleteOldBillsResult.Message, LogLevels.Error, "Data Store", username);
            }
            return deleteOldBillsResult;
        }

        public Result RefreshBillList(string username)
        {
            Result refreshBillsResult = _dao.RefreshBillList();
            if (refreshBillsResult.IsSuccessful)
            {
                _logger.Log("Refresh bill was successful", LogLevels.Info, "Data Store", username, new UserOperation(Operations.BudgetBar, 1));
            }
            else
            {
                _logger.Log(refreshBillsResult.Message, LogLevels.Error, "Data Store", username);
            }
            return refreshBillsResult;
        }

        public Result InsertBill( Bill bill)
        {
            DAOResult result = _dao.InsertBill(bill);
            if (result.IsSuccessful)
            {
                _logger.Log("Add bill was successful", LogLevels.Info, "Data Store", bill.Owner);
            }
            else
            {
                _logger.Log( "Add bill Error: " + result.ErrorStatus  + "\n" +"Message: " + result.Message + "\n" + "State: " + result.SqlState, LogLevels.Error, "Data Store", bill.Owner);
            }
            return result;
        }

         public Result UpdateBill(Bill bill)
        {
             _logger.Log("Updateeeeeee", LogLevels.Info, "Data Store", bill.Owner);
            DAOResult result = _dao.UpdateBill(bill);
            if (result.IsSuccessful)
            {
                _logger.Log("Edit bill was successful", LogLevels.Info, "Data Store", bill.Owner);
            }
            else
            {
                _logger.Log( "Edit bill Error: " + result.ErrorStatus  + "\n" +"Message: " + result.Message + "\n" + "State: " + result.SqlState, LogLevels.Error, "Data Store", bill.Owner);
            }
            return result;
        }

        public decimal GetBudget(int groupId)
        {
            try
            {
                decimal result = _dao.GetBudget(groupId);
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

        public Result DeleteBill(int billId)
        {

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

        public Result RestoreDeletedBill(int billId)
        {
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

        public decimal CalculateGroupTotal(List<Bill> activeBill) 
        {
            Decimal totalSpent = 0;
            Dictionary<string, decimal> totalsByBillId= new Dictionary<string, decimal>();
            foreach(Bill bill in activeBill)
            {
                totalSpent += bill.Amount;
            }
            return totalSpent;
        }

        public decimal CalculateTotal(List<Bill> bills) 
        {
            decimal total = 0;
            foreach (Bill bill in bills)
            {
                total += bill.Amount;
            }
            return total;
        }

        public List<Bill> GetBills(int groupId)
        {
            try
            {
                List<Bill> bills = _dao.GetBills(groupId);
                return bills;
            }
            catch(MySqlException exc)
            {
                _logger.Log( "Error: " + exc.ErrorCode  + "\n" +"Message: " + exc.Message + "\n" + "State: " + exc.SqlState, LogLevels.Error, "Data Store", "Group ID: " + groupId);
                throw exc;
            }
        }
    }
}