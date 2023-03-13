using Microsoft.AspNetCore.Mvc;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;
using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.Services.Implementations;

namespace HAGSJP.WeCasa.Frontend.Controllers;

[ApiController]
[Route("[controller]")]
public class BudgetBarController : Controller
{
    private readonly Logger _logger;
    private readonly BudgetBarManager _budgetBarManager;
    TaskCompletionSource<IActionResult> tcs;

    public BudgetBarController()
    {
        _logger = new Logger(new AccountMariaDAO());
        _budgetBarManager = new BudgetBarManager();
        tcs = new TaskCompletionSource<IActionResult>(TaskCreationOptions.RunContinuationsAsynchronously);
    }

    [HttpGet("{groupId}")]
    public Task<IActionResult> Get([FromRoute]int groupId)
    {
        try
        { 
            Object result = _budgetBarManager.GetInitialBudgetBarVew(groupId);
            tcs.SetResult(Ok(result));
            return tcs.Task;
        }
        catch(Exception exc)
        {
            _logger.Log( "Error Message: " + exc.Message, LogLevels.Error, "Data Store", "Group ID: " + groupId, new UserOperation(Operations.BudgetBar,0));
            tcs.SetResult(NotFound());
            return tcs.Task;
        }
    }

    [Route("EditBill")]
    [HttpPut]
    public Task<IActionResult> Put([FromBody] Bill bill)
    {
        try
        {
            Result editBillResult = _budgetBarManager.UpdateBill(bill);
            if (!editBillResult.IsSuccessful)
            {
                tcs.SetResult(BadRequest());
                return tcs.Task;
            }
            tcs.SetResult(Ok(true));
            return tcs.Task;
        }
        catch(Exception exc)
        {
            _logger.Log( "Error Message: " + exc.Message, LogLevels.Error, "Data Store", bill.Owner, new UserOperation(Operations.BudgetBar,0));
            tcs.SetResult(StatusCode(500));
            return tcs.Task;
        }
    }

    [Route("AddBill")]
    [HttpPost]
    public Task<IActionResult> Post([FromBody] Bill bill)
    {
        try
        {
            Result result = _budgetBarManager.InsertBill(bill);
            if (!result.IsSuccessful) {
                tcs.SetResult(BadRequest());
                return tcs.Task;
            }
            tcs.SetResult(Ok(true));
            return tcs.Task;
        }
        catch(Exception exc)
        {
            _logger.Log( "Error Message: " + exc.Message, LogLevels.Error, "Data Store", bill.Owner, new UserOperation(Operations.BudgetBar,0));
            tcs.SetResult(StatusCode(500));
            return tcs.Task;
        }
    }

    [Route("UpdateBudget")]
    [HttpPut]
    public Task<IActionResult> Put([FromBody] UpdateBudgetRequest request)
    {
        try
        {
            Result editBudgetResult = _budgetBarManager.EditBudget(request.GroupId, request.Amount);
            if (!editBudgetResult.IsSuccessful)
            {
                tcs.SetResult(BadRequest());
                return tcs.Task;
            }
            tcs.SetResult(Ok(true));
            return tcs.Task;
        }
        catch(Exception exc)
        {
            _logger.Log( "Error Message: " + exc.Message, LogLevels.Error, "Data Store", "GroupID: " + request.GroupId, new UserOperation(Operations.BudgetBar,0));
            tcs.SetResult(StatusCode(500));
            return tcs.Task;
        }
    }


    [HttpDelete("Delete/{billId}")]
    public Task<IActionResult> Delete([FromRoute] int billId)
    {
        try
        {
            Result deleteBillResult = _budgetBarManager.DeleteBill(billId);
            if (!deleteBillResult.IsSuccessful)
            {
                tcs.SetResult(BadRequest());
                return tcs.Task;
            }
            tcs.SetResult(Ok(true));
            return tcs.Task;
        }
        catch(Exception exc)
        {
            _logger.Log( "Error Message: " + exc.Message, LogLevels.Error, "Data Store", "Bill Id: " + billId, new UserOperation(Operations.BudgetBar,0));
            tcs.SetResult(StatusCode(500));
            return tcs.Task;
        }
    }

    [HttpPut("Restore/{billId}")]
    public Task<IActionResult> Put([FromRoute] int billId)
    {
        try
        {
            Result restoreDeleteBillResult = _budgetBarManager.RestoreDeletedBill(billId);
            if (!restoreDeleteBillResult.IsSuccessful)
            {
                tcs.SetResult(BadRequest());
                return tcs.Task;
            }
            tcs.SetResult(Ok(true));
            return tcs.Task;
          }
        catch(Exception exc)
        {
            _logger.Log( "Error Message: " + exc.Message, LogLevels.Error, "Data Store", "Bill Id: " + billId, new UserOperation(Operations.BudgetBar,0));
            tcs.SetResult(StatusCode(500));
            return tcs.Task;
        }
    }
}