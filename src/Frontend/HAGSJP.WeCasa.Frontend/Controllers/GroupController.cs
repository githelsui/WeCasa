using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HAGSJP.WeCasa.Client;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Managers.Implementations;
using HAGSJP.WeCasa.Services.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace HAGSJP.WeCasa.Frontend.Controllers
{
    [ApiController]
    [Route("group-settings")]
    public class GroupController : ControllerBase
    {
        private readonly GroupManager _manager;

        public GroupController()
        {
            _manager = new GroupManager();
        }

        [HttpPost]
        [Route("AddGroupMembers")]
        public Result AddGroupMembers([FromBody] GroupMemberForm groupForm)
        {
            var groupMember = groupForm.GroupMember;
            var groupModel = new GroupModel();
            groupModel.GroupId = groupForm.GroupId;
            return _manager.AddGroupMember(groupModel, groupMember);
        }

        [HttpPost]
        [Route("RemoveGroupMembers")]
        public Result RemoveGroupMembers([FromBody] GroupMemberForm groupForm)
        {
            var groupMember = groupForm.GroupMember;
            var groupModel = new GroupModel();
            groupModel.GroupId = groupForm.GroupId;
            return _manager.RemoveGroupMember(groupModel, groupMember);
        }

        [HttpPost]
        [Route("GetGroupMembers")]
        public async Task<GroupResult> GetGroupMembers([FromBody] GroupMemberForm groupForm)
        {
            var groupResult = new GroupResult();
            var groupModel = new GroupModel();
            try
            {
                // check if progress bar is enabled on group
                groupModel.GroupId = groupForm.GroupId;
                groupResult = await _manager.GetGroupMembers(groupModel);
            }
            catch (Exception ex)
            {
                groupResult.IsSuccessful = false;
                groupResult.Message = ex.Message;
            }
            return groupResult;
        }

        [HttpPost]
        [Route("DeleteGroup")]
        public Result DeleteGroup([FromBody] GroupForm groupForm)
        {
            var group = new GroupModel();
            group.GroupId = groupForm.GroupId;
            var result = _manager.DeleteGroup(group);
            return result;
        }

        [HttpPost]
        [Route("EditGroup")]
        public GroupResult EditGroup([FromBody] GroupForm groupForm)
        {
            var newGroup = new GroupModel(groupForm.GroupId, groupForm.Owner, groupForm.GroupName, groupForm.Icon, groupForm.Features); ;
            var result = _manager.EditGroup(groupForm.GroupId, newGroup);
            return result;
        }
    }
}

