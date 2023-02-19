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

        public object GetInitialBudgetBarVew(string username)
        {
            GroupDAO groupDao = new GroupDAO();

            RefreshBillList(username);
            DeleteAllOutdatedBills(username);

            decimal budget = GetBudget(username);
            List<string> usernames = groupDao.GetMembersUsername(username);
            Decimal totalSpent = CalculateGroupTotal(usernames);

            Dictionary<string, decimal> totalSpentPerMember = new Dictionary<string, decimal>();
            foreach(string user in usernames)
            {
                totalSpentPerMember.Add(user, CalculateTotal(GetBills(username, 0)));
            }

            Dictionary<string, List<Bill>> activeBills = GetAllBills(usernames, 0);
            Dictionary<string, List<Bill>> deletedBills = GetAllBills(usernames, 1);
            
            return new {
                Group = usernames,
                Budget = budget,
                GroupTotal = totalSpent,
                TotalSpentPerMember = totalSpentPerMember,
                ActiveBills = activeBills,
                DeletedBills = deletedBills
            };
        }

        // public object GetTableInformation(string username)
        // {
        //     UserAccount ua = new UserAccount(username);
        //     GroupDAO groupDao = new GroupDAO();

        //     _budgetBarManager.DeleteAllOutdatedBills();
        //     List<Bill> activeBills = _budgetBarManager.GetBills(username, 0);
        //     List<Bill> deletedBills = _budgetBarManager.GetBills(username, 1);
            
        //     return new object{
        //         Username = username,
        //         ActiveBills = activeBills,
        //         DeletedBills = deletedBills
        //     };
        // }

        public Boolean InsertBills(List<string> usernames, Bill bill)
        {
            bill.BillId = generateID();
            Result insertBillResult = new Result();
            foreach(string username in usernames)
            {
                bill.Username = username;
                insertBillResult = InsertBill(bill);
                if (!insertBillResult.IsSuccessful)
                {
                    return false;
                }
            }
            return true;
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

        public Result InsertBill(Bill bill)
        {
            DAOResult result = _dao.InsertBill(bill);
            if (result.IsSuccessful)
            {
                _logger.Log("Add bill was successful", LogLevels.Info, "Data Store", bill.Username);
            }
            else
            {
                _logger.Log( "Add bill Error: " + result.ErrorStatus  + "\n" +"Message: " + result.Message + "\n" + "State: " + result.SqlState, LogLevels.Error, "Data Store", bill.Username);
            }
            return result;
        }

         public Result UpdateBill(Bill bill)
        {
            DAOResult result = _dao.UpdateBill(bill);
            if (result.IsSuccessful)
            {
                _logger.Log("Edit bill was successful", LogLevels.Info, "Data Store", bill.Username, new UserOperation(Operations.BudgetBar, 1));
            }
            else
            {
                _logger.Log( "Edit bill Error: " + result.ErrorStatus  + "\n" +"Message: " + result.Message + "\n" + "State: " + result.SqlState, LogLevels.Error, "Data Store", bill.Username);
            }
            return result;
        }

        public decimal GetBudget(string username)
        {
            try
            {
                GroupDAO groupDao = new GroupDAO();
                decimal result = _dao.GetBudget(groupDao.GetGroupId(username));
                _logger.Log("Get budget was successful", LogLevels.Info, "Data Store", username, new UserOperation(Operations.BudgetBar, 1));
                return result;
            }
             catch(MySqlException exc)
            {
                throw exc;
            }
        }

        public Result EditBudget(string username, decimal amount)
        {
            GroupDAO groupDao = new GroupDAO();
            DAOResult result = _dao.EditBudget(groupDao.GetGroupId(username), amount);
            if (result.IsSuccessful)
            {
                _logger.Log("Edit budget was successful", LogLevels.Info, "Data Store", username);
            }
            else
            {
                _logger.Log( "Edit budget Error: " + result.ErrorStatus  + "\n" +"Message: " + result.Message + "\n" + "State: " + result.SqlState, LogLevels.Error, "Data Store", username);
            }
            return result;
        }

        public Result UpdatePaymentStatus(string username, string billId, Boolean paymentStatus)
        {
            GroupDAO groupDao = new GroupDAO();
            DAOResult result = _dao.UpdatePaymentStatus(username, billId, paymentStatus);
            if (result.IsSuccessful)
            {
                _logger.Log("Update Payment Status was successful", LogLevels.Info, "Data Store", username);
            }
            else
            {
                _logger.Log( "Update Payment Status Error: " + result.ErrorStatus  + "\n" +"Message: " + result.Message + "\n" + "State: " + result.SqlState, LogLevels.Error, "Data Store", username);
            }
            return result;
        }

        public Result DeleteBill(string billId)
        {

            DAOResult result = _dao.DeleteBill(billId);
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

        public Result RestoreDeletedBill(string billId)
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

        public decimal CalculateGroupTotal(Dictionary<string, decimal> activeBills) 
        {
            Decimal totalSpent = 0;
            foreach(Bill bill in activeBills)
            {
                totalSpentGroup += bill.Amount;
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

        public List<Bill> GetBills(string username, int isDeleted)
        {
            try
            {
                List<Bill> bills = _dao.GetBills(username, isDeleted);
                return bills;
            }
            catch(MySqlException exc)
            {
                _logger.Log( "Error: " + exc.ErrorCode  + "\n" +"Message: " + exc.Message + "\n" + "State: " + exc.SqlState, LogLevels.Error, "Data Store", username);
                throw exc;
            }
        }

        public Dictionary<string, List<Bill>> GetAllBills(List<string> usernames, int isDeleted)
        {
            try
            {
                Dictionary<string, List<Bill>> allBills = new Dictionary<string, List<Bill>>();
                BudgetBarDAO dao = new BudgetBarDAO();
                foreach(string username in usernames)
                {
                    List<Bill> bills = dao.GetBills(username, isDeleted);
                    allBills.Add(username, bills);
                }
                return allBills;
            }
            catch(MySqlException exc)
            {
                throw exc;
            }
        }

        // public Dictionary<string, List<Bill>> SortBillsByUsername(List<Bill> bills, List<string> usernames)
        // {
        //     Dictionary<string, List<Bill>> sortedBills = new Dictionary<string,  List<Bill>>();
        //     foreach(Bill bill in bills)
        //     {
        //         if (sortedBills[bill.Username] == null)
        //         {
        //             sortedBills.Add(bill.Username, new List<Bill>());
        //         }
        //         sortedBills[bill.Username].Add(bill);
        //     }
        //     return sortedBills;
        // }
    }
}