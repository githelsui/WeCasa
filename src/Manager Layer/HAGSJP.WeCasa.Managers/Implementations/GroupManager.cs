using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.Models.Security;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Net;
using System;
using System.Collections;
using Microsoft.AspNetCore.Identity;

namespace HAGSJP.WeCasa.Managers.Implementations
{
    public class GroupManager : IGroupManager
    {
        private readonly GroupMariaDAO _dao;
        private readonly FilesS3DAO _s3dao;
        private Logger successLogger;
        private Logger errorLogger;

        public GroupManager()
        {
            _dao = new GroupMariaDAO();
            _s3dao = new FilesS3DAO();
            successLogger = new Logger(_dao);
            errorLogger = new Logger(_dao);
        }
        public GroupManager(GroupMariaDAO dao)
        {
            _dao = dao;
            _s3dao = new FilesS3DAO();
            successLogger = new Logger(dao);
            errorLogger = new Logger(dao);
        }

        public GroupResult GetGroups(UserAccount userAccount)
        {
            var result = new GroupResult();
            var groups = _dao.GetGroupList(userAccount).Groups;
            var groupsArr = groups.ToArray();
            result.ReturnedObject = groupsArr;
            return result;
        }
        public GroupResult CreateGroup(GroupModel group)
        {
            // System log entry recorded if group creation process takes longer than 5 seconds
            var stopwatch = new Stopwatch();
            var expected = 5;

            stopwatch.Start();
            var createGroupResult = new GroupResult();
            var createFileBucketResult = new S3Result();

            createGroupResult = _dao.CreateGroup(group);
            createFileBucketResult = _s3dao.CreateBucket(group.GroupId.ToString()).Result;

            if (createGroupResult.IsSuccessful && createFileBucketResult.IsSuccessful)
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
        public Result DeleteGroup(GroupModel group)
        {
            // System log entry recorded if group creation process takes longer than 5 seconds
            var stopwatch = new Stopwatch();
            var expected = 5;

            stopwatch.Start();
            var deleteGroupResult = new Result();
            var deleteGroupFiles = new S3Result();
            var deleteFileBucketResult = new S3Result();

            deleteGroupResult = _dao.DeleteGroup(group);
            deleteGroupFiles = _s3dao.DeleteAllObjects(group.GroupId.ToString()).Result;
            if (deleteGroupResult.IsSuccessful && deleteGroupFiles.IsSuccessful)
            {
                deleteFileBucketResult = _s3dao.DeleteBucket(group.GroupId.ToString()).Result;
                if (deleteFileBucketResult.IsSuccessful)
                {
                    // Logging the group deletion
                    successLogger.Log("Group deleted successfully", LogLevels.Info, "Data Store", group.Owner);
                }
                else
                {
                    // Logging the error
                    errorLogger.Log("Error deleting a group", LogLevels.Error, "Data Store", group.Owner);
                }
            } else
            {
                // Logging the error
                errorLogger.Log("Error deleting group files.", LogLevels.Error, "Data Store", group.Owner);
            }

            stopwatch.Stop();
            var actual = Decimal.Divide(stopwatch.ElapsedMilliseconds, 60_000);
            if (deleteGroupResult.IsSuccessful && actual > expected)
            {
                errorLogger.Log("Group deleted successfully, but took longer than 5 seconds", LogLevels.Info, "Business", group.Owner, new UserOperation(Operations.GroupCreation, 1));
            }

            return deleteGroupResult;
        }

        public GroupResult EditGroup(int groupId, GroupModel newGroup)
        {
            // System log entry recorded if group editing process takes longer than 5 seconds
            var stopwatch = new Stopwatch();
            var expected = 5;

            stopwatch.Start();
            var result = new GroupResult();

            result = _dao.EditGroup(groupId, newGroup);

            if (result.IsSuccessful)
            {
                // Logging the group creation
                successLogger.Log("Group edited successfully", LogLevels.Info, "Data Store", newGroup.Owner);
            }
            else
            {
                // Logging the error
                errorLogger.Log("Error editing group", LogLevels.Error, "Data Store", newGroup.Owner);
            }

            stopwatch.Stop();
            var actual = Decimal.Divide(stopwatch.ElapsedMilliseconds, 60_000);
            if (result.IsSuccessful && actual > expected)
            {
                errorLogger.Log("Group edited successfully, but took longer than 5 seconds", LogLevels.Info, "Business", newGroup.Owner, new UserOperation(Operations.GroupCreation, 1));
            }

            return result;
        }

        public GroupResult GetGroupMembers(GroupModel group)
        {
            var userManager = new UserManager();
            var result = new GroupResult();

            var usernamesList = (_dao.GetGroupMembers(group).ReturnedObject);
            IEnumerable enumerable = usernamesList as IEnumerable;
            var groupMembersList = new List<UserProfile>();
            foreach (string username in enumerable)
            {
                Console.Write("Username = " + username);
                var userAccount = new UserAccount(username);
                var userProfile = userManager.GetUserProfile(userAccount);
                groupMembersList.Add((UserProfile)userProfile.ReturnedObject);
            }
            var groupMemberArr = groupMembersList.ToArray();
            result.ReturnedObject = groupMemberArr;
            return result;
        }

        public Result AddGroupMembers(GroupModel group, string[] groupMembers)
        {
            var result = new Result();
            for(var i = 0; i < groupMembers.Length; i++)
            {
                result = AddGroupMember(group, groupMembers[i]);
                if(!result.IsSuccessful)
                {
                    return result;
                }
            }
            return result;
        }

        public Result AddGroupMember(GroupModel group, string newGroupMember)
        {
            var userManager = new UserManager();
            var result = new Result();

            var groupMemberValid = ValidateGroupMemberInvitation(newGroupMember);
            if (!groupMemberValid.IsSuccessful)
            {
                return groupMemberValid;
            }

            //check if newGroupMember is not owner of group
            //TODO: GroupModel should always have group.Owner attached when calling GetGroups from fontend
            if (group.Owner != null)
            {
                if (group.Owner.Equals(newGroupMember))
                {
                    result.IsSuccessful = false;
                    result.Message = "Cannot add a user who already belongs to the group.";
                    return result;
                }
            }

            //check if newGroupMember already belongs in current group
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

        public Result RemoveGroupMember(GroupModel group, string groupMember)
        {
            var userManager = new UserManager();
            var result = new Result();

            var groupMemberValid = ValidateGroupMemberInvitation(groupMember);
            if (!groupMemberValid.IsSuccessful)
            {
                return groupMemberValid;
            }

            result = _dao.RemoveGroupMember(group, groupMember);

            if (result.IsSuccessful)
            {
                result.Message = "Successfully removed " + groupMember + " from the group.";
                // Logging the group creation
                successLogger.Log("Group member removed successfully", LogLevels.Info, "Data Store", group.Owner);
            }
            else
            {
                result.Message = "Error removing " + groupMember + " from the group.";
                // Logging the error
                errorLogger.Log("Error adding a removing member", LogLevels.Error, "Data Store", group.Owner);
            }

            return result;
        }

        public Result ValidateGroupMemberInvitation(string newGroupMember)
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
            result.IsSuccessful = true;
            return result;
        }
    }
}