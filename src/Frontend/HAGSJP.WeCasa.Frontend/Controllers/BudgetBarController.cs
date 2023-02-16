using Microsoft.AspNetCore.Mvc;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;
using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.Services.Implementations;
using MySqlConnector;
using HAGSJP.WeCasa.Models.Security;
using HAGSJP.WeCasa.sqlDataAccess.Abstractions;

namespace HAGSJP.WeCasa.Frontend.Controllers;

[ApiController]
[Route("[controller]")]
public class BudgetBarController : Controller
{
    private readonly BudgetBarDAO _budgetBarDao;
    private readonly Logger _logger;
    private readonly BudgetBarManager _budgetBarManager;

    public BudgetBarController()
    {
        _budgetBarDao = new BudgetBarDAO();
        _logger = new Logger(new AccountMariaDAO());
        _budgetBarManager = new BudgetBarManager();
    }

    [HttpGet("{username}")]
    public IActionResult Get([FromRoute]string username)
    {
        var request = new {
                Username = "testacc@gmail.com",
                GroupId = "some-group-id"
            };
         try
        {  
            // AuthorizationService auth = new AuthorizationService(new AuthorizationDAO());
            // AccountMariaDAO mariaDAO = new AccountMariaDAO();
            UserAccount ua = new UserAccount(request.Username);
            GroupDAO groupDao = new GroupDAO();
            Claim claim = new Claim("Read", "Read files");

            // Boolean authorized = auth.ValidateClaim(ua, claim).ReturnedObject != null ? (bool) auth.ValidateClaim(ua, claim).ReturnedObject : false;
            // Boolean authenticated = mariaDAO.ValidateUserInfo(ua).IsAuth;
            // if (authorized && authenticated)
            // {
                _budgetBarManager.RefreshBillList();
                decimal budget = _budgetBarManager.GetBudget(request.GroupId);
                List<string> usernames = groupDao.GetMembersUsername(request.GroupId);
                Decimal totalSpent = _budgetBarManager.CalculateGroupTotal(usernames);
                Dictionary<string, decimal> totalSpentPerMember = new Dictionary<string, decimal>();

                foreach(string user in usernames)
                {
                    totalSpentPerMember.Add(user, _budgetBarManager.CalculateTotal(_budgetBarManager.GetBills(username, 0)));
                }

                InitialBudgetBarViewResponse response = new InitialBudgetBarViewResponse{
                    Group = usernames,
                    Budget = budget,
                    GroupTotal = totalSpent,
                    TotalSpentPerMember = totalSpentPerMember
                };
                return Ok(response);
            // }
            // else
            // {
            //     _logger.Log( "Unauthorized Access", LogLevels.Error, "Data Store", request.Username);
            //     return Unauthorized();
            // }
        }
        catch(MySqlException exc)
        {
            _logger.Log( "Error: " + exc.ErrorCode  + "\n" +"Message: " + exc.Message + "\n" + "State: " + exc.SqlState, LogLevels.Error, "Data Store", request.Username);
            return BadRequest();
        }
        catch(Exception exc)
        {
            _logger.Log( "Error Message: " + exc.Message, LogLevels.Error, "Data Store", request.Username);
            return NotFound();
        }
    }


    [HttpGet("{username}")]
    public IActionResult GetTable([FromRoute]string username)
    {
        var request = new {
            Username = "testacc@gmail.com",
            GroupId = "some-group-id"
        };
        // AuthorizationService auth = new AuthorizationService(new AuthorizationDAO());
        // AccountMariaDAO mariaDAO = new AccountMariaDAO();
        UserAccount ua = new UserAccount(request.Username);
        GroupDAO groupDao = new GroupDAO();
        Claim claim = new Claim("Read", "Read files");
        try
        {           
            // Boolean authorized = auth.ValidateClaim(ua, claim).ReturnedObject != null ? (bool) auth.ValidateClaim(ua, claim).ReturnedObject : false;
            // Boolean authenticated = mariaDAO.ValidateUserInfo(ua).IsAuth;
            // if (authorized && authenticated)
            // {
                _budgetBarManager.DeleteAllOutdatedBills();
                List<Bill> activeBills = _budgetBarManager.GetBills(username, 0);
                List<Bill> deletedBills = _budgetBarManager.GetBills(username, 1);
                
                UserTableViewResponse response = new UserTableViewResponse{
                    Username = username,
                    ActiveBills = activeBills,
                    DeletedBills = deletedBills
                };
                return Ok(response);
            // }
            // else
            // {
            //     _logger.Log( "Unauthorized Access", LogLevels.Error, "Data Store", request.Username);
            //     return Unauthorized();
            // }
        }
        catch(MySqlException exc)
        {
            _logger.Log( "Error: " + exc.ErrorCode  + "\n" +"Message: " + exc.Message + "\n" + "State: " + exc.SqlState, LogLevels.Error, "Data Store", request.Username);
            return BadRequest();
        }
        catch(Exception exc)
        {
            _logger.Log( "Error Message: " + exc.Message, LogLevels.Error, "Data Store", request.Username);
            return NotFound();
        }
    }

    // TODO: add method for updating group bills, and personal 
    [Route("EditBill")]
    [HttpPut]
    public IActionResult Put([FromBody] Bill bill)
    {
        Result editBillResult = _budgetBarManager.UpdateBill(bill);
        if (!editBillResult.IsSuccessful)
        {
            return BadRequest();
        }
        return Ok(true);
    }

    [Route("AddBill")]
    [HttpPost]
    public IActionResult Post([FromBody] AddBillRequest request)
    {
        // TODO: add billid
        Result insertBillResult = new Result();
        foreach(string username in request.Usernames)
        {
           request.Bill.Username = username;
           insertBillResult = _budgetBarManager.InsertBill(request.Bill);
            if (!insertBillResult.IsSuccessful)
            {
                return BadRequest();
            }
        }
        return Ok(true);
    }

    [Route("UpdateBudget")]
    [HttpPut]
    public IActionResult Put([FromBody] UpdateBudgetRequest request)
    {
        Result editBudgetResult = _budgetBarManager.EditBudget( request.GroupId, request.Amount);
        if (!editBudgetResult.IsSuccessful)
        {
            return BadRequest();
        }
        return Ok(true);
    }

    [HttpDelete("Delete")]
    public IActionResult Delete([FromBody] string billId)
    {
        Result deleteBillResult = _budgetBarManager.DeleteBill(billId, DateTime.Now);
        if (!deleteBillResult.IsSuccessful)
        {
            return BadRequest();
        }
        return Ok(true);
    }

    [Route("Restore")]
    [HttpPut]
    public IActionResult Put([FromBody] string billId)
    {
        Result restoreDeleteBillResult = _budgetBarManager.RestoreDeletedBill(billId);
        if (!restoreDeleteBillResult.IsSuccessful)
        {
            return BadRequest();
        }
        return Ok(true);
    }
}