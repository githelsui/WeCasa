using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.Models.Security;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Net;

namespace HAGSJP.WeCasa.Services.Implementations
{
    public class GroupManager : IGroupManager
    {
        private readonly GroupMariaDAO _dao;
        private Logger successLogger;
        private Logger errorLogger;

        public GroupManager()
        {
            _dao = new GroupMariaDAO();
            successLogger = new Logger(_dao);
            errorLogger = new Logger(_dao);
        }
        public GroupManager(GroupMariaDAO dao)
        {
            _dao = dao;
            successLogger = new Logger(dao);
            errorLogger = new Logger(dao);
        }

        public Result GetGroups(UserAccount userAccount)
        {
            // System log entry recorded if fetching groups takes longer than 5 seconds
            var stopwatch = new Stopwatch();
            var expected = 5;

            stopwatch.Start();
            var groupListResult = _dao.GetGroupList(userAccount);

            if (!groupListResult.IsSuccessful)
            {
                errorLogger.Log(groupListResult.Message, LogLevels.Error, "Data Store", userAccount.Username);
            }
            stopwatch.Stop();
            var actual = Decimal.Divide(stopwatch.ElapsedMilliseconds, 60_000);
            if (groupListResult.IsSuccessful && actual > expected)
            {
                errorLogger.Log("List of groups retrieved successfully, but took longer than 5 seconds", LogLevels.Info, "Business", userAccount.Username);
            }

            return groupListResult;
            
        }
        public Result CreateGroup(GroupModel group)
        {
            // System log entry recorded if group creation process takes longer than 5 seconds
            var stopwatch = new Stopwatch();
            var expected = 5;

            stopwatch.Start();
            var createGroupResult = new Result();

            createGroupResult = _dao.CreateGroup(group);

            if (createGroupResult.IsSuccessful)
            {
                // Logging the group creation
                successLogger.Log("Group created successfully", LogLevels.Info, "Data Store", group.Owner);
            }
            else
            {
                // Logging the error
                errorLogger.Log("Error creating a group", LogLevels.Error, "Data Store", group.Owner);
            }

            stopwatch.Stop();
            var actual = Decimal.Divide(stopwatch.ElapsedMilliseconds, 60_000);
            if (createGroupResult.IsSuccessful && actual > expected)
            {
                errorLogger.Log("Group created successfully, but took longer than 5 seconds", LogLevels.Info, "Business", group.Owner, new UserOperation(Operations.GroupCreation, 1));
            }

            return createGroupResult;
        }
        public Result DeleteGroup(UserAccount userAccount, GroupModel group)
        {
            throw new NotImplementedException();
        }
        public Result EditGroup(UserAccount userAccount, int groupId, GroupModel newGroup)
        {
            throw new NotImplementedException();
        }

        public Result AddGroupMember(GroupModel group, string newGroupMember)
        {
            var userManager = new UserManager();
            var result = new Result();

            // check if valid email
            var emailValidation = userManager.ValidateEmail(newGroupMember);
            if (!emailValidation.IsSuccessful)
            {
                return emailValidation;
            }

            // check if account exists
            var existingAcc = userManager.IsUsernameTaken(newGroupMember);
            if (!existingAcc)
            {
                result.IsSuccessful = false;
                result.Message = "Cannot add a user that does not exist.";
                return result;
            }

            //check if newGroupMember is not owner of group
            //TODO: GroupModel should always have group.Owner attached when calling GetGroups from fontend
            if(group.Owner != null)
            {
                if (group.Owner.Equals(newGroupMember))
                {
                    result.IsSuccessful = false;
                    result.Message = "Cannot add a user who already belongs to the group.";
                    return result;
                }
            }

            //heck if newGroupMember already belongs in current group
            var userInGroup = _dao.FindGroupMember(group, newGroupMember);
            if ((bool)userInGroup.ReturnedObject)
            {
                result.IsSuccessful = false;
                result.Message = "Cannot add a user who already belongs to the group.";
                return result;
            }

            //if all validations pass -> dao.AddGroupMember(group, newGroupMember)
            result = _dao.AddGroupMember(group, newGroupMember);

            if (result.IsSuccessful)
            {
                // Logging the group creation
                successLogger.Log("Group member added successfully", LogLevels.Info, "Data Store", group.Owner);
            }
            else
            {
                // Logging the error
                errorLogger.Log("Error adding a group member", LogLevels.Error, "Data Store", group.Owner);
            }

            return result;
        }
    }
}