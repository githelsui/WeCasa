using Microsoft.AspNetCore.Mvc;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;
using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.Managers.Implementations;

namespace HAGSJP.WeCasa.Frontend.Controllers;

[ApiController]
[Route("bulletin-board")]
public class BulletinBoardController : ControllerBase
{
    private readonly Logger _logger;
    private readonly BulletinBoardManager _bulletinBoardManager;
    TaskCompletionSource<IActionResult> tcs;

    public BulletinBoardController()
    {
        _logger = new Logger(new AccountMariaDAO());
        _bulletinBoardManager = new BulletinBoardManager();
        tcs = new TaskCompletionSource<IActionResult>(TaskCreationOptions.RunContinuationsAsynchronously);
    }


    [HttpGet("{groupId}")]
    public Task<IActionResult> Get([FromRoute]int groupId)
    {
        try
        { 
            Object result = _bulletinBoardManager.GetNotes(groupId);
            tcs.SetResult(Ok(result));
            return tcs.Task;
        }
        catch(Exception exc)
        {
            tcs.SetResult(NotFound());
            return tcs.Task;
        }
    }

    [Route("Add")]
    [HttpPost]
    public Task<IActionResult> Post([FromBody] Note note)
    {
        return ControllerTemplate(() => _bulletinBoardManager.AddNote(note));
    }

    [HttpDelete("Delete/{noteId}")]
    public Task<IActionResult> Delete([FromRoute] int noteId)
    {
       return ControllerTemplate(() => _bulletinBoardManager.DeleteNote(noteId));
    }


    [Route("Update")]
    [HttpPost]
    public Task<IActionResult> PostUpdate([FromBody] Note note)
    {
        return ControllerTemplate(() => _bulletinBoardManager.UpdateNote(note));
    }

    public Task<IActionResult> ControllerTemplate(Func<Result> managerFunc)
    {
        try
        {
            Result result = managerFunc();
            if (!result.IsSuccessful)
            {
                tcs.SetResult(NotFound());
                Console.Write("NOt successful");
            }
            else
            {
                tcs.SetResult(Ok(true));
                Console.Write(result.Message);
            }
        }
        catch(Exception exc)
        {
            Console.Write("Exception");
            tcs.SetResult(StatusCode(500));
        }
        return tcs.Task;
    }
}