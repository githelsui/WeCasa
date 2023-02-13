using Microsoft.AspNetCore.Mvc;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;
using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.Services.Implementations;
using HAGSJP.WeCasa.Models.Security;
using MySqlConnector;

namespace HAGSJP.WeCasa.Frontend.Controllers;

[ApiController]
[Route("[controller]")]
public class BudgetBarController : Controller
{
    private readonly BudgetBarDAO _budgetBarDao;
    private readonly Logger _logger;

    public BudgetBarController()
    {
        _budgetBarDao = new BudgetBarDAO();
    }

    [HttpGet]
    public Boolean Get()
    {
        return true;
    }

//     [HttpGet]
//     public Boolean Get([FromBody] string username, string groupId)
//     {
//         try
//         {
//             BudgetBarHelper BudgetBarHelper = new BudgetBarHelper();
//             AuthorizationService auth = new AuthorizationService(new AuthorizationDAO());
//             AccountMariaDAO mariaDAO = new AccountMariaDAO();
//             UserAccount ua = new UserAccount(username);
//             GroupDAO groupDao = new GroupDAO();

//             Boolean authorized = (bool) auth.ValidateClaim(ua, new Claim("Read", "Read files")).ReturnedObject;
//             Boolean authenticated = mariaDAO.ValidateUserInfo(ua).IsAuth;

//             Result deleteOldBillsResult = _budgetBarDao.DeleteAllOutdatedBills();
//             Result refreshBillsResult = _budgetBarDao.RefreshBillList();

//             decimal budget = _budgetBarDao.GetBudget(groupId);

//             List<Bill> activeBills = _budgetBarDao.GetBills( groupId, 1);
//             List<Bill> deleteBills = _budgetBarDao.GetBills( groupId, 0);
//             _logger.Log( "Got bills successfully", LogLevels.Error, "Data Store", username);

//             decimal total = BudgetBarHelper.CalculateTotal(activeBills);

//             List<string> usernames = groupDao.GetMembersUsername(groupId);
//             Dictionary<string, List<Bill>> sortedActiveBills = BudgetBarHelper.SortBillsByUsername(activeBills, usernames);
//             Dictionary<string, List<Bill>> sortedDeleteBills = BudgetBarHelper.SortBillsByUsername(deleteBills, usernames);
//         }
//         catch(MySqlException exc)
//         {
//             _logger.Log( "Error: " + exc.ErrorCode  + "\n" +"Message: " + exc.Message + "\n" + "State: " + exc.SqlState, LogLevels.Error, "Data Store", username);
//         }
//         return true;
//     }

//     [Route("edit-bill")]
//     [HttpPut]
//     // public Result Put([FromBody] Bill bill)
//     public Boolean Put([FromBody] Bill bill)
//     {
//         DAOResult editBillResult = _budgetBarDao.UpdateBill(bill);
//         if (editBillResult.IsSuccessful)
//         {
//            _logger.Log("Edit bill was successful", LogLevels.Info, "Data Store", bill.Username);
//         }
//         else
//         {
//            _logger.Log("Edit bill was unsuccessful", LogLevels.Info, "Data Store", bill.Username);
//         }
//         // return editBillResult;
//         return true;
//     }

//     [Route("add-bill")]
//     [HttpPost]
//     public Result Post([FromBody] List <string> usernames, Bill bill)
//     {
//         // TODO: add billid
//         Result insertBillResult = new Result();
//         foreach(string username in usernames)
//         {
//            bill.Username = username;
//            insertBillResult = _budgetBarDao.InsertBill(bill);
//            if (insertBillResult.IsSuccessful)
//            {
//                _logger.Log("Insert bill was successful", LogLevels.Info, "Data Store", bill.BillId);
//            }
//            else
//            {
//                _logger.Log("Insert bill was unsuccessful", LogLevels.Info, "Data Store",  bill.BillId);
//            }
//         }
//         return insertBillResult;
//     }

//     [Route("update-budget")]
//     [HttpPost]
//     public Result Post([FromBody] string groupID, decimal amount)
//     {
//         Result editBudgetResult = _budgetBarDao.EditBudget( groupID, amount);
//         if (editBudgetResult.IsSuccessful)
//         {
//             _logger.Log("Edit Budget was successful", LogLevels.Info, "Data Store", groupID);
//         }
//         else
//         {
//             _logger.Log("Edit Budget was unsuccessful", LogLevels.Info, "Data Store", groupID);
//         }
//         return editBudgetResult;
//     }

//     [HttpDelete]
//     public Result Delete([FromBody] Bill bill)
//     {
//         Result deleteBillResult = _budgetBarDao.DeleteBill(bill);
//         if (deleteBillResult.IsSuccessful)
//         {
//             _logger.Log("Delete bill was successful", LogLevels.Info, "Data Store", bill.Username);
//         }
//         else
//         {
//             _logger.Log("Delete bill was unsuccessful", LogLevels.Info, "Data Store", bill.Username);
//         }
//         return deleteBillResult;
//     }

//     [Route("restore")]
//     [HttpPut]
//     public Result Put([FromBody] string billId)
//     {
//         Result restoreDeleteBillResult = _budgetBarDao.RestoreDeletedBill(billId);
//         if (restoreDeleteBillResult.IsSuccessful)
//         {
//             _logger.Log("Restore bill was successful", LogLevels.Info, "Data Store", billId);
//         }
//         else
//         {
//             _logger.Log("Restore bill was unsuccessful", LogLevels.Info, "Data Store", billId);
//         }
//         return restoreDeleteBillResult;
//     }
// }