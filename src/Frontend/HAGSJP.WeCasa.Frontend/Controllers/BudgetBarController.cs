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
    private readonly Logger _logger;
    private readonly BudgetBarManager _budgetBarManager;

    public BudgetBarController()
    {
        _logger = new Logger(new AccountMariaDAO());
        _budgetBarManager = new BudgetBarManager();
    }

    //     public BudgetBarController(BudgetBarManager budgetBarManager)
    // {
    //     _budgetBarDao = new BudgetBarDAO();
    //     _logger = new Logger(new AccountMariaDAO());
    //     _budgetBarManager = new BudgetBarManager();
    // }

    // Add try catch

    // [HttpGet("{username}")]
    // //public Task<IActionResult> Get([FromRoute]string username)
    // public IActionResult Get([FromRoute] string groupId)
    // {
    //     var tcs = new TaskCompletionSource<IActionResult>(TaskCreationOptions.RunContinuationsAsynchronously);
    //      try
    //     {  
    //         // AuthorizationService auth = new AuthorizationService(new AuthorizationDAO());
    //         // AccountMariaDAO mariaDAO = new AccountMariaDAO();
    //         // UserAccount ua = new UserAccount(request.Username);
    //         GroupDAO groupDao = new GroupDAO();
    //         // Claim claim = new Claim("Read", "Read files");

    //         // Boolean authorized = auth.ValidateClaim(ua, claim).ReturnedObject != null ? (bool) auth.ValidateClaim(ua, claim).ReturnedObject : false;
    //         // Boolean authenticated = mariaDAO.ValidateUserInfo(ua).IsAuth;
    //         // if (authorized && authenticated)
    //         // {
    //             _budgetBarManager.RefreshBillList();
    //             decimal budget = _budgetBarManager.(request.GroupId);
    //             List<string> usernames = groupDao.GetMembersUsername(request.GroupId);
    //             Decimal totalSpent = _budgetBarManager.CalculateGroupTotal(usernames);
    //             Dictionary<string, decimal> totalSpentPerMember = new Dictionary<string, decimal>();

    //             foreach(string user in usernames)
    //             {
    //                 totalSpentPerMember.Add(user, _budgetBarManager.CalculateTotal(_budgetBarManager.GetBills(username, 0)));
    //             }

    //             InitialBudgetBarViewResponse response = new InitialBudgetBarViewResponse{
    //                 Group = usernames,
    //                 Budget = budget,
    //                 GroupTotal = totalSpent,
    //                 TotalSpentPerMember = totalSpentPerMember
    //             };
    //             return Ok(response);

    //             //var response = new HttpRes
    //             //tcs.SetResult((IActionResult)response)
    //             //return tcs.Task;
    //         // }
    //         // else
    //         // {
    //         //     _logger.Log( "Unauthorized Access", LogLevels.Error, "Data Store", request.Username);
    //         //     return Unauthorized();
    //         // }
    //     }
    //     catch(MySqlException exc)
    //     {
    //         _logger.Log( "Error: " + exc.ErrorCode  + "\n" +"Message: " + exc.Message + "\n" + "State: " + exc.SqlState, LogLevels.Error, "Data Store", request.Username);
    //         return BadRequest();
    //     }
    //     catch(Exception exc)
    //     {
    //         _logger.Log( "Error Message: " + exc.Message, LogLevels.Error, "Data Store", request.Username);
    //         return NotFound();
    //     }
    // }


    // [HttpGet("{username}")]
    // public Task<IActionResult> Get([FromRoute]string username)
    // {
    //     var tcs = new TaskCompletionSource<IActionResult>(TaskCreationOptions.RunContinuationsAsynchronously);
    //     try
    //     {   
    //         tcs.SetResult(Ok(_budgetBarManager.GetInitialBudgetBarVew(username)));
    //         return tcs.Task;
    //     }
    //     catch(MySqlException exc)
    //     {
    //         _logger.Log( "Error: " + exc.ErrorCode  + "\n" +"Message: " + exc.Message + "\n" + "State: " + exc.SqlState, LogLevels.Error, "Data Store", username);
    //         return BadRequest();
    //     }
    //     catch(Exception exc)
    //     {
    //         _logger.Log( "Error Message: " + exc.Message, LogLevels.Error, "Data Store", username);
    //         return NotFound();
    //     }
    // }

    [HttpGet("{username}")]
    public Task<IActionResult> Get([FromRoute]string username)
    {
        var tcs = new TaskCompletionSource<IActionResult>(TaskCreationOptions.RunContinuationsAsynchronously);
        try
        {   
            tcs.SetResult(Ok(_budgetBarManager.GetInitialBudgetBarVew(username)));
            return tcs.Task;
        }
        catch(MySqlException exc)
        {
             _logger.Log( "Error: " + exc.ErrorCode  + "\n" +"Message: " + exc.Message + "\n" + "State: " + exc.SqlState, LogLevels.Error, "Data Store", username);

            _logger.Log( "Error: " + exc.ErrorCode  + "\n" +"Message: " + exc.Message + "\n" + "State: " + exc.SqlState, LogLevels.Error, "Data Store", username);
            tcs.SetResult(BadRequest());
            return tcs.Task;
        }
        catch(Exception exc)
        {
            _logger.Log( "Error Message: " + exc.Message, LogLevels.Error, "Data Store", username);
            tcs.SetResult(NotFound());
            return tcs.Task;
        }
    }

    [Route("EditBill")]
    [HttpPut]
    public Task<IActionResult> Put([FromBody] Bill bill)
    {
        var tcs = new TaskCompletionSource<IActionResult>(TaskCreationOptions.RunContinuationsAsynchronously);
        Result editBillResult = _budgetBarManager.UpdateBill(bill);
        if (!editBillResult.IsSuccessful)
        {
            tcs.SetResult(BadRequest());
            return tcs.Task;
        }
        tcs.SetResult(Ok(true));
        return tcs.Task;
    }

    [Route("UpdatePaymentStatus")]
    [HttpPut]
    public Task<IActionResult> Put([FromBody] UpdatePaymentStatusRequest request)
    {
        var tcs = new TaskCompletionSource<IActionResult>(TaskCreationOptions.RunContinuationsAsynchronously);

        Result editBillResult = _budgetBarManager.UpdatePaymentStatus(request.Username, request.BillId, request.PaymentStatus);
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
        try
        {
            Boolean result = _budgetBarManager.InsertBills(request.Usernames, request.Bill);
            return Ok(result);
        }
        catch(Exception exc)
        {
            _logger.Log( "Error Message: " + exc.Message, LogLevels.Error, "Data Store", request.Bill.Username);
            return NotFound();
        }
    }

    [Route("UpdateBudget")]
    [HttpPut]
    public IActionResult Put([FromBody] UpdateBudgetRequest request)
    {
        try 
        {
            Result editBudgetResult = _budgetBarManager.EditBudget( request.Username, request.Amount);
            if (!editBudgetResult.IsSuccessful)
            {
                return BadRequest();
            }
            return Ok(true);
        }
        catch(Exception exc)
        {
            _logger.Log( "Error Message: " + exc.Message, LogLevels.Error, "Data Store", request.Username);
            return NotFound();
        }
    }


    [HttpDelete("Delete")]
    public IActionResult Delete([FromBody] string billId)
    {
        try
        {
            Result deleteBillResult = _budgetBarManager.DeleteBill(billId);
            if (!deleteBillResult.IsSuccessful)
            {
                return BadRequest();
            }
            return Ok(true);
          }
        catch(Exception exc)
        {
            _logger.Log( "Error Message: " + exc.Message, LogLevels.Error, "Data Store", "Bill Id: " + billId);
            return NotFound();
        }
    }

    [Route("Restore")]
    [HttpPut]
    public IActionResult Put([FromBody] string billId)
    {
        try
        {
            Result restoreDeleteBillResult = _budgetBarManager.RestoreDeletedBill(billId);
            if (!restoreDeleteBillResult.IsSuccessful)
            {
                return BadRequest();
            }
            return Ok(true);
          }
        catch(Exception exc)
        {
            _logger.Log( "Error Message: " + exc.Message, LogLevels.Error, "Data Store", "Bill Id: " + billId);
            return NotFound();
        }
    }
}