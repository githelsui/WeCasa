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
        private readonly GroupManager _groupManager;

        public ChoreController()
        {
            _logger = new Logger(new AccountMariaDAO());
            _manager = new ChoreManager();
            _groupManager = new GroupManager();
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
                chore.ChoreDate = choreForm.ChoreDate;
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
                Chore chore = new Chore(choreForm.ChoreId, choreForm.Name, choreForm.Days, choreForm.Notes, choreForm.GroupId, choreForm.AssignedTo, choreForm.Repeats);
                chore.ChoreDate = choreForm.ChoreDate;
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
        [Route("DeleteChore")]
        public ChoreResult DeleteChore([FromBody] ChoreForm choreForm)
        {
            try
            {
                Chore chore = new Chore(choreForm.ChoreId, choreForm.Name, choreForm.Days, choreForm.Notes, choreForm.GroupId, choreForm.AssignedTo, choreForm.Repeats);
                chore.ChoreDate = choreForm.ChoreDate;
                var result = _manager.DeleteChore(chore, new UserAccount(choreForm.CurrentUser));
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
        public ChoreResult GetGroupToDoChores([FromBody] ChoreForm groupForm)
        {
            try
            {
                var currDate = (groupForm.CurrentDate != null) ? DateTime.Parse(groupForm.CurrentDate) : DateTime.Now;
                var result = _manager.GetGroupToDoChores(new GroupModel(groupForm.GroupId), currDate);
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
        public ChoreResult GetGroupCompletedChores([FromBody] GroupMemberForm groupForm)
        {
            try
            {
                var result = _manager.GetGroupCompletedChores(new GroupModel(groupForm.GroupId));
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
        [Route("GetUserIncompleteChores")]
        public async Task<ChoreResult> GetUserIncompleteChores([FromBody] GroupMemberForm groupForm)
        {
            try
            {
                var group = new GroupModel(groupForm.GroupId, "githelsuico@gmail.com");
                var result = await _manager.GetGroupIncompleteChores(group);
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
            var result = new ChoreResult();
            var groupModel = new GroupModel();
            groupModel.GroupId = groupForm.GroupId;
            try
            {
                var managerResult = await _groupManager.GetGroupMembers(groupModel);
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

