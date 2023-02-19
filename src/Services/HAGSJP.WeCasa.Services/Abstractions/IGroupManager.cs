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
        public Result CreateGroup(UserAccount userAccount, Group group);
        public Result DeleteGroup(UserAccount userAccount, Group group);
        public Result EditGroup(UserAccount userAccount, int groupId, Group newGroup);
    }
}