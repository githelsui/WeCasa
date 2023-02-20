using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HAGSJP.WeCasa.Client;
using HAGSJP.WeCasa.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HAGSJP.WeCasa.Frontend.Controllers
{
    [ApiController]
    [Route("group-settings")]
    public class GroupController : Controller
    {
        [HttpPost]
        [Route("AddGroupMembers")]
        public Result AddGroupMembers([FromBody] GroupMemberForm groupForm)
        {
            var groupMembers = groupForm.GroupMembers;
            var groupId = groupForm.GroupId;
            return new Result();
        }

        [HttpPost]
        [Route("RemoveGroupMembers")]
        public Result RemoveGroupMembers([FromBody] GroupMemberForm groupForm)
        {
            return new Result();
        }
    }
}

