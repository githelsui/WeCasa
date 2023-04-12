using Microsoft.AspNetCore.Mvc;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;
using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.Managers.Implementations;

namespace HAGSJP.WeCasa.Frontend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChoreController : ControllerBase
    {
        private readonly Logger _logger;
        private readonly ChoreManager _manager;

        public ChoreController()
        {
            _logger = new Logger(new AccountMariaDAO());
            _manager = new ChoreManager();
        }

        [HttpPost]
        [Route("AddChore")]
        public ChoreResult AddChore([FromBody] ChoreForm choreForm)
        {
            try
            {
                Chore chore = new Chore();
                chore.Name = choreForm.Name;
                chore.ResetTime = choreForm.ResetTime;
                chore.Notes = choreForm.Notes;
                chore.GroupId = choreForm.GroupId;
                chore.AssignedTo = choreForm.AssignedTo;
                chore.Repeats = choreForm.Repeats;
                var result = _manager.AddChore(chore, new UserAccount(choreForm.CurrentUser));
                if(result.IsSuccessful)
                {
                    result.ErrorStatus = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    result.ErrorStatus = System.Net.HttpStatusCode.BadRequest;
                }
                return result;

            }
            catch(Exception exc)
            {
                return new ChoreResult(false, System.Net.HttpStatusCode.Conflict, exc.Message);
            }
        }
    }
}

