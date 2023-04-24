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
        public ChoreResult AddChore([FromBody] ChoreForm choreForm)
        {
            try
            {
                Chore chore = new Chore(choreForm.Name, choreForm.Days, choreForm.Notes, choreForm.GroupId, choreForm.AssignedTo, choreForm.Repeats);
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
        public ChoreResult EditChore([FromBody] ChoreForm choreForm)
        {
            try
            {
                Chore chore = new Chore(choreForm.ChoreId, choreForm.Name, choreForm.Days, choreForm.Notes, choreForm.GroupId, choreForm.AssignedTo, choreForm.Repeats);
                var result = _manager.EditChore(chore, new UserAccount(choreForm.CurrentUser));
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
        public ChoreResult GetGroupToDoChores([FromBody] GroupMemberForm groupForm)
        {
            try
            {
                var result = _manager.GetGroupToDoChores(new GroupModel(groupForm.GroupId));
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
        [Route("GetUserToDoChores")]
        public ChoreResult GetUserToDoChores([FromBody] AccountForm accForm)
        {
            try
            {
                var result = _manager.GetUserToDoChores(new UserAccount(accForm.Email));
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
        public ChoreResult GetCurrentGroupMembers([FromBody] GroupMemberForm groupForm)
        {
            try
            {
                var result = new ChoreResult();
                var groupModel = new GroupModel();
                groupModel.GroupId = groupForm.GroupId;
                var groupManager = new GroupManager();
                var managerResult = groupManager.GetGroupMembers(groupModel);
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

