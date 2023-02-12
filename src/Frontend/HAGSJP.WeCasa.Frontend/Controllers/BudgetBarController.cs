using Microsoft.AspNetCore.Mvc;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;
using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.Services.Implementations;
using HAGSJP.WeCasa.Models.Security;

namespace HAGSJP.WeCasa.Frontend.Controllers;

[ApiController]
[Route("[controller]")]
public class BudgetBarController : Controller
{
    private readonly BudgetBarHelper _BudgetBarHelper;
    private readonly BudgetBarDAO _budgetBarDao;
    private readonly Logger _logger;

    public BudgetBarController()
    {
        _BudgetBarHelper = new BudgetBarHelper();
        _budgetBarDao = new BudgetBarDAO();
    }

    [HttpGet]
    // public Boolean Get([FromBody] string username, string groupId)
    public Boolean Get([FromBody] string username)
    {
        AuthorizationService auth = new AuthorizationService(new AuthorizationDAO());
        AccountMariaDAO mariaDAO = new AccountMariaDAO();
        UserAccount ua = new UserAccount(username);
        GroupDAO groupDao = new GroupDAO();

        Boolean authorized = (bool) auth.ValidateClaim(ua, new Claim("Read", "Read files")).ReturnedObject;
        Boolean authenticated = mariaDAO.ValidateUserInfo(ua).IsAuth;

        Result deleteOldBillsResult = _budgetBarDao.DeleteAllOutdatedBills();
        Result refreshBillsResult = _budgetBarDao.RefreshBillList();

        // decimal budget = _budgetBarDao.GetBudget(groupId);

        // List<Bill> activeBills = _budgetBarDao.GetBills( groupId, 1);
        // List<Bill> deleteBills = _budgetBarDao.GetBills( groupId, 0);

        // decimal total = _BudgetBarHelper.CalculateTotal(activeBills);

        // List<string> usernames = groupDao.GetMembersUsername(groupId);
        // Dictionary<string, List<Bill>> sortedActiveBills = _BudgetBarHelper.SortBillsByUsername(activeBills, usernames);
        // Dictionary<string, List<Bill>> sortedDeleteBills = _BudgetBarHelper.SortBillsByUsername(deleteBills, usernames);

        return true;
    }

    [Route("edit-bill")]
    [HttpPut]
    // public Result Put([FromBody] string groupId, Bill bill)
    public Boolean Put([FromBody] string groupId)
    {
        GroupDAO groupDao = new GroupDAO();
        List<string> usernames = groupDao.GetMembersUsername(groupId);
        Result editBillResult = new Result();
        foreach(string username in usernames)
        {
            // bill.Username = username;
            // editBillResult = _budgetBarDao.UpdateBill(bill);
            if (editBillResult.IsSuccessful)
            {
                _logger.Log("Edit bill was successful", LogLevels.Info, "Data Store", username);
            }
            else
            {
                _logger.Log("Edit bill was unsuccessful", LogLevels.Info, "Data Store", username);
            }
        }
        // return editBillResult;
        return true;
    }

    [Route("add-bill")]
    [HttpPost]
    // public Result Post([FromBody] string groupId, Bill bill)
    public Boolean Post([FromBody] string groupId)
    {
        GroupDAO groupDao = new GroupDAO();
        List<string> usernames = groupDao.GetMembersUsername(groupId);
        Result insertBillResult = new Result();
        foreach(string username in usernames)
        {
            // bill.Username = username;
            // insertBillResult = _budgetBarDao.InsertBill(bill);
            if (insertBillResult.IsSuccessful)
            {
                _logger.Log("Insert bill was successful", LogLevels.Info, "Data Store", username);
            }
            else
            {
                _logger.Log("Insert bill was unsuccessful", LogLevels.Info, "Data Store", username);
            }
        }
        // return insertBillResult;
        return true;
    }

    // [Route("budget")]
    // [HttpPost]
    // // public Result Post([FromBody] string groupID, decimal amount)
    // public Boolean Post([FromBody] string groupID)
    // {
    //     // Result editBudgetResult = _budgetBarDao.EditBudget( groupID, amount);
    //     if (editBudgetResult.IsSuccessful)
    //     {
    //         _logger.Log("Edit Budget was successful", LogLevels.Info, "Data Store", groupID);
    //     }
    //     else
    //     {
    //         _logger.Log("Edit Budget was unsuccessful", LogLevels.Info, "Data Store", groupID);
    //     }
    //     // return editBudgetResult;
    //     return true;
    // }

    [HttpDelete]
    public Result Delete([FromBody] Bill bill)
    {
        Result deleteBillResult = _budgetBarDao.DeleteBill(bill);
        if (deleteBillResult.IsSuccessful)
        {
            _logger.Log("Delete bill was successful", LogLevels.Info, "Data Store", bill.Username);
        }
        else
        {
            _logger.Log("Delete bill was unsuccessful", LogLevels.Info, "Data Store", bill.Username);
        }
        return deleteBillResult;
    }

    // [Route("restore")]
    // [HttpPut]
    // public Boolean Put([FromBody] string billId)
    // {
    //     Result restoreDeleteBillResult = _budgetBarDao.RestoreDeletedBill(billId);
    //     if (restoreDeleteBillResult.IsSuccessful)
    //     {
    //         _logger.Log("Restore bill was successful", LogLevels.Info, "Data Store", billId);
    //     }
    //     else
    //     {
    //         _logger.Log("Restore bill was unsuccessful", LogLevels.Info, "Data Store", billId);
    //     }
    //     // return restoreDeleteBillResult;
    //     return true;
    // }
}