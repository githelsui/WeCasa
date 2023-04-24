using Microsoft.AspNetCore.Mvc;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;
using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.Managers.Implementations;

namespace HAGSJP.WeCasa.Frontend.Controllers
{
    [ApiController]
    [Route("chorelist")]
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
        public async Task<ChoreResult> AddChore([FromBody] ChoreForm choreForm)
        {
            try
            {
                Chore chore = new Chore(choreForm.Name, choreForm.Days, choreForm.Notes, choreForm.GroupId, choreForm.AssignedTo, choreForm.Repeats);
                var result = await _manager.AddChore(chore, new UserAccount(choreForm.CurrentUser));
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

        [HttpPost]
        [Route("CompleteChore")]
        public ChoreResult CompleteChore([FromBody] ChoreForm choreForm)
        {
            try
            {
                Chore chore = new Chore(choreForm.ChoreId, choreForm.Name, choreForm.Days, choreForm.Notes, choreForm.GroupId, choreForm.AssignedTo, choreForm.Repeats);
                var result = _manager.CompleteChore(chore, new UserAccount(choreForm.CurrentUser));
                if (result.IsSuccessful)
                {
                    result.ErrorStatus = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    result.ErrorStatus = System.Net.HttpStatusCode.BadRequest;
                }
                return result;

            }
            catch (Exception exc)
            {
                return new ChoreResult(false, System.Net.HttpStatusCode.Conflict, exc.Message);
            }
        }

        [HttpPost]
        [Route("EditChore")]
        public async Task<ChoreResult> EditChore([FromBody] ChoreForm choreForm)
        {
            try
            {
                Chore chore = new Chore(choreForm.Name, choreForm.Days, choreForm.Notes, choreForm.GroupId, choreForm.AssignedTo, choreForm.Repeats);
                var result = await _manager.EditChore(chore, new UserAccount(choreForm.CurrentUser));
                if (result.IsSuccessful)
                {
                    result.ErrorStatus = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    result.ErrorStatus = System.Net.HttpStatusCode.BadRequest;
                }
                return result;

            }
            catch (Exception exc)
            {
                return new ChoreResult(false, System.Net.HttpStatusCode.Conflict, exc.Message);
            }
        }

        [HttpPost]
        [Route("GetGroupToDoChores")]
        public ChoreResult GetGroupToDoChores([FromBody] ChoreForm choreForm)
        {
            try
            {
                var result = _manager.GetGroupToDoChores(new GroupModel(choreForm.GroupId));
                if (result.IsSuccessful)
                {
                    result.ErrorStatus = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    result.ErrorStatus = System.Net.HttpStatusCode.BadRequest;
                }
                return result;

            }
            catch (Exception exc)
            {
                return new ChoreResult(false, System.Net.HttpStatusCode.Conflict, exc.Message);
            }
        }

        [HttpPost]
        [Route("GetGroupCompletedChores")]
        public ChoreResult GetGroupCompletedChores([FromBody] ChoreForm choreForm)
        {
            try
            {
                var result = _manager.GetGroupCompletedChores(new GroupModel(choreForm.GroupId));
                if (result.IsSuccessful)
                {
                    result.ErrorStatus = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    result.ErrorStatus = System.Net.HttpStatusCode.BadRequest;
                }
                return result;

            }
            catch (Exception exc)
            {
                return new ChoreResult(false, System.Net.HttpStatusCode.Conflict, exc.Message);
            }
        }

        [HttpPost]
        [Route("GetUserToDoChores")]
        public ChoreResult GetUserToDoChores([FromBody] ChoreForm choreForm)
        {
            try
            {
                var result = _manager.GetUserToDoChores(new UserAccount(choreForm.CurrentUser));
                if (result.IsSuccessful)
                {
                    result.ErrorStatus = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    result.ErrorStatus = System.Net.HttpStatusCode.BadRequest;
                }
                return result;

            }
            catch (Exception exc)
            {
                return new ChoreResult(false, System.Net.HttpStatusCode.Conflict, exc.Message);
            }
        }

        [HttpPost]
        [Route("GetUserCompletedChores")]
        public ChoreResult GetUserCompletedChores([FromBody] ChoreForm choreForm)
        {
            try
            {
                var result = _manager.GetUserCompletedChores(new UserAccount(choreForm.CurrentUser));
                if (result.IsSuccessful)
                {
                    result.ErrorStatus = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    result.ErrorStatus = System.Net.HttpStatusCode.BadRequest;
                }
                return result;

            }
            catch (Exception exc)
            {
                return new ChoreResult(false, System.Net.HttpStatusCode.Conflict, exc.Message);
            }
        }

        [HttpPost]
        [Route("GetCurrentGroupMembers")]
        public async Task<ChoreResult> GetCurrentGroupMembers([FromBody] GroupMemberForm groupForm)
        {
            try
            {
                var result = new ChoreResult();
                var groupModel = new GroupModel();
                groupModel.GroupId = groupForm.GroupId;
                var groupManager = new GroupManager();
                var managerResult = await groupManager.GetGroupMembers(groupModel);
                if (managerResult.IsSuccessful)
                {
                    result.ReturnedObject = managerResult.ReturnedObject;
                    result.ErrorStatus = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    result.ErrorStatus = System.Net.HttpStatusCode.BadRequest;
                }
                result.IsSuccessful = managerResult.IsSuccessful;
                result.Message = managerResult.Message;
                return result;

            }
            catch (Exception exc)
            {
                return new ChoreResult(false, System.Net.HttpStatusCode.Conflict, exc.Message);
            }
        }
    }
}

