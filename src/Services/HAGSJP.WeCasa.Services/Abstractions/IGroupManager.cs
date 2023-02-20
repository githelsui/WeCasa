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
        public Result GetGroups(UserAccount userAccount);
        public GroupResult CreateGroup(GroupModel group);
        public Result DeleteGroup(UserAccount userAccount, GroupModel group);
        public Result EditGroup(UserAccount userAccount, int groupId, GroupModel newGroup);
        public Result AddGroupMember(GroupModel group, string newGroupMember);
        public GroupResult GetGroupMembers(GroupModel group);
        public Result RemoveGroupMember(GroupModel group, string groupMember);
        public Result ValidateGroupMemberInvitation(string newGroupMember);
    }
}