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

        public object GetInitialBudgetBarVew(int groupId)
        {
            Logger lo = new Logger(new GroupMariaDAO());
            AccountMariaDAO dao = new AccountMariaDAO();
            // RefreshBillList(username);
            // DeleteAllOutdatedBills(username);
            lo.Log( "1 "+ groupId, LogLevels.Error, "Data Store", "");
            decimal budget = GetBudget(groupId);
            lo.Log( "2 "+ groupId, LogLevels.Error, "Data Store", "");
            Dictionary<string, string> names = dao.GetFirstNames(groupId);
            lo.Log( "3 "+ groupId, LogLevels.Error, "Data Store", "");
            Dictionary<string, BudgetBarUser> budgetBarUsers = new Dictionary<string, BudgetBarUser>();
            foreach(var name in names)
            {
                BudgetBarUser bbUser = new BudgetBarUser(name.Key, name.Value);
                lo.Log( "4 "+ groupId, LogLevels.Error, "Data Store", "");
                budgetBarUsers.Add(name.Key, bbUser);
            }
            lo.Log( "5 "+ groupId, LogLevels.Error, "Data Store", "");
            List<Bill> bills = GetBills(groupId);
             lo.Log( "6 "+ groupId, LogLevels.Error, "Data Store", "");
            List<int> activeBillIds = new List<int>();
            List<int> deletedBillIds = new List<int>();
            Decimal totalSpent = 0;
            foreach(Bill bill in bills)
            {
                if (bill.IsDeleted == false)
                {
                    // activeBillIds.Add(bill.BillId) ;
                    budgetBarUsers[bill.Owner].ActiveBills.Add(bill);
                    budgetBarUsers[bill.Owner].TotalSpent += bill.Amount;
                    totalSpent += bill.Amount;
                }
                else 
                {
                    // deletedBillIds.Add(bill.BillId) ;
                    budgetBarUsers[bill.Owner].DeletedBills.Add(bill);
                }
            }
            
            return new {
                // ActiveBillIds = activeBillIds,
                // DeletedBillIds = deletedBillIds,
                Group = budgetBarUsers.Values.ToList(),
                Budget = budget,
                GroupTotal = totalSpent,
            };
        }

        // TODO: make view if there is not group id passed in
        // public object GetInitialBudgetBarVew(string username)
        // {
        //     AccountMariaDAO dao = new AccountMariaDAO();
        //     RefreshBillList(username);
        //     DeleteAllOutdatedBills(username);
        //     decimal budget = GetBudget(groupId);
            
        //     Dictionary<string, string> names = dao.GetFirstNames(groupId);
        //     Dictionary<string, BudgetBarUser> budgetBarUsers = new Dictionary<string, BudgetBarUser>();
        //     foreach(var name in names)
        //     {
        //         BudgetBarUser bbUser = new BudgetBarUser(name.Key, name.Value);
        //         budgetBarUsers.Add(name.Key, bbUser);
        //     }

        //     List<Bill> bills = GetBills(groupId);
        //     Decimal totalSpent = 0;
        //     foreach(Bill bill in bills)
        //     {
        //         if (bill.IsDeleted == false)
        //         {
        //             budgetBarUsers[bill.Username].ActiveBills.Add(bill);
        //             budgetBarUsers[bill.Username].TotalSpent += bill.Amount;
        //             totalSpent += bill.Amount;
        //         }
        //         budgetBarUsers[bill.Username].DeletedBills.Add(bill);
        //     }
            
        //     return new {
        //         budgetBarUser = bbUser;
        //     };
        // }

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

        // public Result UpdatePaymentStatus(string username, string billId, Boolean paymentStatus)
        // {
        //     DAOResult result = _dao.UpdatePaymentStatus(username, billId, paymentStatus);
        //     if (result.IsSuccessful)
        //     {
        //         _logger.Log("Update Payment Status was successful", LogLevels.Info, "Data Store", username);
        //     }
        //     else
        //     {
        //         _logger.Log( "Update Payment Status Error: " + result.ErrorStatus  + "\n" +"Message: " + result.Message + "\n" + "State: " + result.SqlState, LogLevels.Error, "Data Store", username);
        //     }
        //     return result;
        // }

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

        // public Dictionary<string, List<Bill>> GetAllBills(List<string> usernames, int isActive)
        // {
        //     try
        //     {
        //         Dictionary<string, List<Bill>> allBills = new Dictionary<string, List<Bill>>();
        //         BudgetBarDAO dao = new BudgetBarDAO();
        //         foreach(string username in usernames)
        //         {
        //             List<Bill> bills = dao.GetBills(username, isActive);
        //             allBills.Add(username, bills);
        //         }
        //         return allBills;
        //     }
        //     catch(MySqlException exc)
        //     {
        //         throw exc;
        //     }
        // }

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