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

    [HttpGet("{groupId}")]
    public Task<IActionResult> Get([FromRoute]int groupId)
    {
        Logger lo = new Logger(new GroupMariaDAO());
        var tcs = new TaskCompletionSource<IActionResult>(TaskCreationOptions.RunContinuationsAsynchronously);
        try
        { 
            tcs.SetResult(Ok(_budgetBarManager.GetInitialBudgetBarVew(groupId)));
            return tcs.Task;
        }
        catch(MySqlException exc)
        {
            _logger.Log( "Error: " + exc.ErrorCode  + "\n" +"Message: " + exc.Message + "\n" + "State: " + exc.SqlState, LogLevels.Error, "Data Store", "Group ID: " + groupId);
            tcs.SetResult(BadRequest());
            return tcs.Task;
        }
        catch(Exception exc)
        {
            _logger.Log( "Error Message: " + exc.Message, LogLevels.Error, "Data Store", "Group ID: " + groupId);
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

    [Route("AddBill")]
    [HttpPost]
    public IActionResult Post([FromBody] Bill bill)
    {
        Logger lo = new Logger(new GroupMariaDAO());
        lo.Log( "ADD BILL", LogLevels.Error, "Data Store", bill.Owner);
        try
        {
            lo.Log( "ADD 2", LogLevels.Error, "Data Store", bill.Owner);
            Result result = _budgetBarManager.InsertBill(bill);
            return Ok(result.IsSuccessful);
        }
        catch(Exception exc)
        {
            _logger.Log( "Error Message: " + exc.Message, LogLevels.Error, "Data Store", bill.Owner);
            return NotFound();
        }
    }

    [Route("UpdateBudget")]
    [HttpPut]
    public IActionResult Put([FromBody] UpdateBudgetRequest request)
    {
        Logger lo = new Logger(new GroupMariaDAO());
        lo.Log( "UPDATE BILL", LogLevels.Error, "Data Store", "");
        try
        {
            lo.Log( "UPDATE 2", LogLevels.Error, "Data Store","" );
            Result editBudgetResult = _budgetBarManager.EditBudget(request.GroupId, request.Amount);
            if (!editBudgetResult.IsSuccessful)
            {
                _logger.Log( "Update Budget Error Message: ", LogLevels.Error, "Data Store", "GroupID: " + request.GroupId);
                return BadRequest();
            }
            return Ok(true);
        }
        catch(Exception exc)
        {
            _logger.Log( "Error Message: " + exc.Message, LogLevels.Error, "Data Store", "GroupID: " + request.GroupId);
            return NotFound();
        }
    }


    [HttpDelete("Delete/{billId}")]
    public IActionResult Delete([FromRoute] int billId)
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

    [HttpPut("Restore/{billId}")]
    public IActionResult Put([FromRoute] int billId)
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