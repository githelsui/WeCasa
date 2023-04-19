using Microsoft.AspNetCore.Mvc;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;
using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.Managers.Implementations;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace HAGSJP.WeCasa.Frontend.Controllers
{
    [ApiController]
    [Route("grocerylist")]
    public class GroceryController : ControllerBase
    {
        private readonly Logger _logger;
        private readonly GroceryManager _manager;

        public GroceryController()
        {
            _logger = new Logger(new AccountMariaDAO());
            _manager = new GroceryManager();
        }

        [HttpPost]
        [Route("AddGroceryItem")]
        public GroceryResult AddGroceryItem([FromBody] GroceryForm groceryForm)
        {
            GroceryItem item = new GroceryItem(groceryForm.GroupId, groceryForm.Name, (groceryForm.Notes != null ? groceryForm.Notes : ""), (groceryForm.Assignments != null ? groceryForm.Assignments : new List<string>()));
            var result = _manager.AddGroceryItem(item, new UserAccount(groceryForm.CurrentUser));
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

        [HttpPost]
        [Route("GetGroceryItems")]
        public GroceryResult GetGroceryItems([FromBody] GroupMemberForm groupForm)
        {
             var result = _manager.GetGroceryItems(new GroupModel(groupForm.GroupId));
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

        [HttpPost]
        [Route("PurchaseItem")]
        public GroceryResult PurchaseItem([FromBody] GroceryForm groceryForm)
        {
            var result = new GroceryResult();
            GroceryItem item = new GroceryItem(groceryForm.GroupId, groceryForm.Name, (groceryForm.Notes != null ? groceryForm.Notes : ""), (groceryForm.Assignments != null ? groceryForm.Assignments : new List<string>()));
            //TODO: Implement Controller -> Manager -> Service -> DAO for PurchaseItem
            //var result = _manager.PurchaseItem(item, new UserAccount(groceryForm.CurrentUser));
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

        [HttpPost]
        [Route("GetCurrentGroupMembers")]
        public GroceryResult GetCurrentGroupMembers([FromBody] GroupMemberForm groupForm)
        {
            try
            {
                var result = new GroceryResult();
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
                return new GroceryResult(false, System.Net.HttpStatusCode.Conflict, exc.Message);
            }
        }
    }
}