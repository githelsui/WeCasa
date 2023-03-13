using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using HAGSJP.WeCasa.Models.Security;

namespace HAGSJP.WeCasa.Services.Implementations
{
    public interface IGroupManager
    {
        public GroupResult GetGroups(UserAccount userAccount);
        public GroupResult CreateGroup(GroupModel group);
        public GroupResult EditGroup(int groupId, GroupModel newGroup);
        public Result DeleteGroup(GroupModel group);
        public Result AddGroupMember(GroupModel group, string newGroupMember);
        public GroupResult GetGroupMembers(GroupModel group);
        public Result RemoveGroupMember(GroupModel group, string groupMember);
        public Result ValidateGroupMemberInvitation(string newGroupMember);
    }
}