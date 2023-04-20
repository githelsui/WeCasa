﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HAGSJP.WeCasa.Client;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Managers.Implementations;
using HAGSJP.WeCasa.Services.Implementations;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HAGSJP.WeCasa.Frontend.Controllers
{
    [ApiController]
    [Route("group-settings")]
    public class GroupController : ControllerBase
    {
        [HttpPost]
        [Route("AddGroupMembers")]
        public Result AddGroupMembers([FromBody] GroupMemberForm groupForm)
        {
            var groupMember = groupForm.GroupMember;
            var groupModel = new GroupModel();
            groupModel.GroupId = groupForm.GroupId;
            var groupManager = new GroupManager();
            return groupManager.AddGroupMember(groupModel, groupMember);
        }

        [HttpPost]
        [Route("RemoveGroupMembers")]
        public Result RemoveGroupMembers([FromBody] GroupMemberForm groupForm)
        {
            var groupMember = groupForm.GroupMember;
            var groupModel = new GroupModel();
            groupModel.GroupId = groupForm.GroupId;
            var groupManager = new GroupManager();
            return groupManager.RemoveGroupMember(groupModel, groupMember);
        }

        [HttpPost]
        [Route("GetGroupMembers")]
        public async Task<GroupResult> GetGroupMembers([FromBody] GroupMemberForm groupForm)
        {
            var groupModel = new GroupModel();
            var result = new GroupResult();
            try
            {
                groupModel.GroupId = groupForm.GroupId;
                var groupManager = new GroupManager();
                result = groupManager.GetGroupMembers(groupModel);
            }
            catch (Exception ex)
            {
                result.IsSuccessful = false;
                result.Message = ex.Message;
            }
            return result;
        }

        [HttpPost]
        [Route("DeleteGroup")]
        public Result DeleteGroup([FromBody] GroupForm groupForm)
        {
            var group = new GroupModel();
            group.GroupId = groupForm.GroupId;
            var groupManager = new GroupManager();
            var result = groupManager.DeleteGroup(group);
            return result;
        }

        [HttpPost]
        [Route("EditGroup")]
        public GroupResult EditGroup([FromBody] GroupForm groupForm)
        {
            var newGroup = new GroupModel(groupForm.GroupId, groupForm.Owner, groupForm.GroupName, groupForm.Icon, groupForm.Features); ;
            var groupManager = new GroupManager();
            var result = groupManager.EditGroup(groupForm.GroupId, newGroup);
            return result;
        }
    }
}

