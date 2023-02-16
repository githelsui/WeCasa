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

        public Result DeleteAllOutdatedBills()
        {
            Result deleteOldBillsResult = _dao.DeleteAllOutdatedBills();
            if (!deleteOldBillsResult.IsSuccessful)
            {
                string host = Dns.GetHostName();
                IPHostEntry ip = Dns.GetHostEntry(host);
                _logger.Log(deleteOldBillsResult.Message, LogLevels.Error, "Data Store", ip.AddressList[0].ToString());
            }
            return deleteOldBillsResult;
        }

        public Result RefreshBillList()
        {
            Result refreshBillsResult = _dao.RefreshBillList();
            if (!refreshBillsResult.IsSuccessful)
            {
                string host = Dns.GetHostName();
                IPHostEntry ip = Dns.GetHostEntry(host);
                _logger.Log(refreshBillsResult.Message, LogLevels.Error, "Data Store", ip.AddressList[0].ToString());
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
                _logger.Log("Edit bill was successful", LogLevels.Info, "Data Store", bill.Username);
            }
            else
            {
                _logger.Log( "Edit bill Error: " + result.ErrorStatus  + "\n" +"Message: " + result.Message + "\n" + "State: " + result.SqlState, LogLevels.Error, "Data Store", bill.Username);
            }
            return result;
        }

        public decimal GetBudget(string groupId)
        {
            try
            {
                decimal result = _dao.GetBudget(groupId);
                return result;
            }
             catch(MySqlException exc)
            {
                _logger.Log( "Error: " + exc.ErrorCode  + "\n" +"Message: " + exc.Message + "\n" + "State: " + exc.SqlState, LogLevels.Error, "Data Store", groupId);
                throw exc;
            }
        }

        public Result EditBudget(string groupId, decimal amount)
        {
            DAOResult result = _dao.EditBudget(groupId, amount);
            if (result.IsSuccessful)
            {
                _logger.Log("Edit budget was successful", LogLevels.Info, "Data Store", groupId);
            }
            else
            {
                _logger.Log( "Edit budget Error: " + result.ErrorStatus  + "\n" +"Message: " + result.Message + "\n" + "State: " + result.SqlState, LogLevels.Error, "Data Store", groupId);
            }
            return result;
        }

        public Result DeleteBill(string billId, DateTime date)
        {
            DAOResult result = _dao.DeleteBill(billId, date);
            if (result.IsSuccessful)
            {
                _logger.Log("Delete bill was successful", LogLevels.Info, "Data Store", billId);
            }
            else
            {
                _logger.Log( "Delete bill Error: " + result.ErrorStatus  + "\n" +"Message: " + result.Message + "\n" + "State: " + result.SqlState, LogLevels.Error, "Data Store", billId);
            }
            return result;
        }

        public Result RestoreDeletedBill(string groupId)
        {
            DAOResult result = _dao.RestoreDeletedBill(groupId);
            if (result.IsSuccessful)
            {
                _logger.Log("Restore bill was successful", LogLevels.Info, "Data Store", groupId);
            }
            else
            {
                _logger.Log( "Restore bill Error: " + result.ErrorStatus  + "\n" +"Message: " + result.Message + "\n" + "State: " + result.SqlState, LogLevels.Error, "Data Store", groupId);
            }
            return result;
        }

        public decimal CalculateGroupTotal(List<string> usernames) 
        {
            Decimal totalSpent = 0;
            Dictionary<string, decimal> totalsByBillId= new Dictionary<string, decimal>();
            foreach(string username in usernames)
            {
                List<Bill> bills = GetBills( username,  0);
                foreach(Bill bill in bills)
                {
                    if (totalsByBillId.ContainsKey(bill.BillId))
                    {
                        totalsByBillId[bill.BillId] = bill.Amount;
                    }
                    else 
                    {
                        totalsByBillId.Add(bill.BillId, bill.Amount);
                        totalSpent += bill.Amount;
                    }
                }
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