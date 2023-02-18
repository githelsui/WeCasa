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
        public Result CreateGroup(UserAccount userAccount, Group group)
        {
            throw new NotImplementedException();
        }
        public Result DeleteGroup(UserAccount userAccount, Group group)
        {
            throw new NotImplementedException();
        }
        public Result EditGroup(UserAccount userAccount, int groupId, Group newGroup)
        {
            throw new NotImplementedException();
        }
    }
}