using System;
using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;
using HAGSJP.WeCasa.Services.Implementations;
using MySqlX.XDevAPI.CRUD;
using MySqlX.XDevAPI.Common;
using System.Collections.Generic;
using System.Collections;
using System.Globalization;
using Mysqlx.Session;
using static Mysqlx.Expect.Open.Types.Condition.Types;
using System.Diagnostics.Metrics;

namespace HAGSJP.WeCasa.Managers.Implementations
{
    public class ChoreManager
    {
        private readonly UserManager _um;
        private readonly ChoreService _service;
        private readonly GroupManager _groupManager;
        private Logger _logger;
        private RemindersDAO remindersDao;


        public ChoreManager()
        {
            _logger = new Logger(new AccountMariaDAO());
            _service = new ChoreService();
            _um = new UserManager();
            _groupManager = new GroupManager();
        }

        public async Task<ChoreResult> AddChore(Chore chore, UserAccount userAccount)
        {
            try
            {
                var result = new ChoreResult();

                chore.Created = DateTime.Now;
                chore.CreatedBy = userAccount.Username;
                chore.IsCompleted = false;

                var assignedProfilesRes = await ReassignChore(chore, chore.UsernamesAssignedTo);
                if (assignedProfilesRes.IsSuccessful)
                {
                    chore.AssignedTo = (List<UserProfile>)assignedProfilesRes.ReturnedObject;
                }
                else
                {
                    result.IsSuccessful = false;
                    result.Message = assignedProfilesRes.Message;
                    return result;
                }

                var serviceResult = _service.AddChore(chore);
                if (serviceResult.IsSuccessful)
                {
                    result.ReturnedObject = serviceResult.ReturnedObject;
                    _logger.Log("Add chore was successful", LogLevels.Info, "Service", userAccount.Username);
                }
                else
                {
                    _logger.Log("Add chore error: " + result.ErrorStatus + "\n" + "Message: " + result.Message, LogLevels.Error, "Service", userAccount.Username);
                }
                result.IsSuccessful = serviceResult.IsSuccessful;
                result.Message = serviceResult.Message;
                return result;
            }
            catch (Exception exc)
            {
                _logger.Log("Error Message: " + exc.Message, LogLevels.Error, "Service", userAccount.Username, new UserOperation(Operations.ChoreList, 0));
                throw exc;
            }
        }

        public async Task<ChoreResult> EditChore(Chore chore, UserAccount userAccount)
        {
            try
            {
                var result = new ChoreResult();

                chore.LastUpdated = DateTime.Now;
                chore.LastUpdatedBy = userAccount.Username;

                var assignedProfilesRes = await ReassignChore(chore, chore.UsernamesAssignedTo);
                if (assignedProfilesRes.IsSuccessful)
                {
                    chore.AssignedTo = (List<UserProfile>)assignedProfilesRes.ReturnedObject;
                }
                else
                {
                    result.IsSuccessful = false;
                    result.Message = assignedProfilesRes.Message;
                    return result;
                }

                var serviceResult = _service.EditChore(chore);
                if (serviceResult.IsSuccessful)
                {
                    result.ReturnedObject = serviceResult.ReturnedObject;
                    _logger.Log("Edit chore was successful", LogLevels.Info, "Service", userAccount.Username);
                }
                else
                {
                    _logger.Log("Edit chore error: " + result.ErrorStatus + "\n" + "Message: " + result.Message, LogLevels.Error, "Service", userAccount.Username);
                }
                result.IsSuccessful = serviceResult.IsSuccessful;
                result.Message = serviceResult.Message;
                return result;
            }
            catch (Exception exc)
            {
                _logger.Log("Error Message: " + exc.Message, LogLevels.Error, "Service", userAccount.Username, new UserOperation(Operations.ChoreList, 0));
                throw exc;
            }
        }

        public ChoreResult CompleteChore(Chore chore, UserAccount userAccount)
        {
            try
            {
                var result = new ChoreResult();

                chore.LastCompleted = DateTime.Now;
                chore.IsCompleted = true;

                var serviceResult = _service.CompleteChore(chore);
                if (serviceResult.IsSuccessful)
                {
                    result.ReturnedObject = serviceResult.ReturnedObject;
                    _logger.Log("Chore completion saved successfully", LogLevels.Info, "Data Store", userAccount.Username);
                }
                else
                {
                    _logger.Log("Chore completion error: " + result.ErrorStatus + "\n" + "Message: " + result.Message, LogLevels.Error, "Service", userAccount.Username);
                }
                result.IsSuccessful = serviceResult.IsSuccessful;
                result.Message = serviceResult.Message;
                return result;
            }
            catch (Exception exc)
            {
                _logger.Log("Error Message: " + exc.Message, LogLevels.Error, "Service", userAccount.Username, new UserOperation(Operations.ChoreList, 0));
                throw exc;
            }
        }

        public ChoreResult DeleteChore(Chore chore, UserAccount userAccount)
        {
            try
            {
                var result = new ChoreResult();

                chore.LastUpdated = DateTime.Now;
                chore.LastUpdatedBy = userAccount.Username;
                chore.IsCompleted = true;

                var serviceResult = _service.DeleteChore(chore);
                if (serviceResult.IsSuccessful)
                {
                    result.ReturnedObject = serviceResult.ReturnedObject;
                    _logger.Log("Chore deletion successful", LogLevels.Info, "Data Store", userAccount.Username);
                }
                else
                {
                    _logger.Log("Chore deletion error: " + result.ErrorStatus + "\n" + "Message: " + result.Message, LogLevels.Error, "Service", userAccount.Username);
                }
                result.IsSuccessful = serviceResult.IsSuccessful;
                result.Message = serviceResult.Message;
                return result;
            }
            catch (Exception exc)
            {
                _logger.Log("Error Message: " + exc.Message, LogLevels.Error, "Service", userAccount.Username, new UserOperation(Operations.ChoreList, 0));
                throw exc;
            }
        }

        public async Task<ChoreResult> UndoChore(Chore chore, UserAccount userAccount)
        {
            try
            {
                var result = new ChoreResult();

                chore.LastUpdated = DateTime.Now;
                chore.LastUpdatedBy = userAccount.Username;
                chore.IsCompleted = false;

                var assignedProfilesRes = await ReassignChore(chore, chore.UsernamesAssignedTo);
                if (assignedProfilesRes.IsSuccessful)
                {
                    chore.AssignedTo = (List<UserProfile>)assignedProfilesRes.ReturnedObject;
                }
                else
                {
                    result.IsSuccessful = false;
                    result.Message = assignedProfilesRes.Message;
                    return result;
                }


                var serviceResult = _service.EditChore(chore);
                if (serviceResult.IsSuccessful)
                {
                    result.ReturnedObject = serviceResult.ReturnedObject;
                    _logger.Log("Chore completion saved successfully", LogLevels.Info, "Service", userAccount.Username);
                }
                else
                {
                    _logger.Log("Chore completion error: " + result.ErrorStatus + "\n" + "Message: " + result.Message, LogLevels.Error, "Service", userAccount.Username);
                }
                result.IsSuccessful = serviceResult.IsSuccessful;
                result.Message = serviceResult.Message;
                return result;
            }
            catch (Exception exc)
            {
                _logger.Log("Error Message: " + exc.Message, LogLevels.Error, "Service", userAccount.Username, new UserOperation(Operations.ChoreList, 0));
                throw exc;
            }
        }

        public ChoreResult GetGroupToDoChores(GroupModel group, DateTime currentDate)
        {
            try
            {
                var result = new ChoreResult();

                var serviceResult = _service.GetGroupWeeklyToDoChores(group, currentDate); //Returns list of chores based on current week
                if (serviceResult.IsSuccessful)
                {
                    List<Chore> resultQuery = (List<Chore>)serviceResult.ReturnedObject;
                    var choresPerDay = new Dictionary<string, List<Chore>>()
                    {
                        {"MON", new List<Chore>() },
                        {"TUES", new List<Chore>() },
                        {"WED", new List<Chore>() },
                        {"THURS", new List<Chore>() },
                        {"FRI", new List<Chore>() },
                        {"SAT", new List<Chore>() },
                        {"SUN", new List<Chore>() }
                    };

                    for (var i = 0; i < resultQuery.Count; i++)
                    {
                        var chore = resultQuery[i];
                        var days = (List<String>)chore.Days;
                        var isCompleted = (bool)chore.IsCompleted;
                        DateTime choreDate = (DateTime)chore.ChoreDate;
                        var day = choreDate.DayOfWeek;
                        var key = "";

                        if (day == DayOfWeek.Monday)
                        {
                            key = "MON";
                        }

                        if (day == DayOfWeek.Tuesday)
                        {
                            key = "TUES";
                        }

                        if (day == DayOfWeek.Wednesday)
                        {
                            key = "WED";
                        }

                        if (day == DayOfWeek.Thursday)
                        {
                            key = "THURS";
                        }

                        if (day == DayOfWeek.Friday)
                        {
                            key = "FRI";
                        }

                        if (day == DayOfWeek.Saturday)
                        {
                            key = "SAT";
                        }

                        if (day == DayOfWeek.Sunday)
                        {
                            key = "SUN";
                        }

                        //if a chore with choreId & choreDate doesnt already exist in choresPerDay[key] then
                        if (NoDuplicateChores(choresPerDay[key], chore))
                        {
                            choresPerDay[key].Add(chore);
                        }

                    }
                    result.ReturnedObject = choresPerDay;
                    _logger.Log("Group to-do chores fetched successfully", LogLevels.Info, "Service", group.Owner);
                }
                else
                {
                    _logger.Log("Chore fetch error: " + result.ErrorStatus + "\n" + "Message: " + result.Message, LogLevels.Error, "Service", group.Owner);
                }
                result.IsSuccessful = serviceResult.IsSuccessful;
                result.Message = serviceResult.Message;
                return result;
            }
            catch (Exception exc)
            {
                _logger.Log("Error Message: " + exc.Message, LogLevels.Error, "Service", group.Owner, new UserOperation(Operations.ChoreList, 0));
                throw exc;
            }
        }

        public ChoreResult GetGroupCompletedChores(GroupModel group)
        {
            try
            {
                var result = new ChoreResult();

                var serviceResult = _service.GetGroupCompletedChores(group);
                if (serviceResult.IsSuccessful)
                {
                    List<Chore> resultQuery = (List<Chore>)serviceResult.ReturnedObject;
                    var choresPerDay = new Dictionary<string, List<Chore>>();
                    for (var i = 0; i < resultQuery.Count; i++)
                    {
                        var chore = resultQuery[i];
                        var dateCompleted = chore.ChoreDate;
                        string key = string.Format("{0:dddd MM/dd/yy}", dateCompleted);
                        if (choresPerDay.ContainsKey(key))
                        {
                            if (NoDuplicateChores(choresPerDay[key], chore))
                            {
                                choresPerDay[key].Add(chore);
                            }
                        }
                        else
                        {
                            choresPerDay.Add(key, new List<Chore>() { chore });
                        }
                    }
                    result.ReturnedObject = choresPerDay;
                    _logger.Log("Group completed chores fetched successfully", LogLevels.Info, "Service", group.Owner);
                }
                else
                {
                    _logger.Log("Chore fetch error: " + result.ErrorStatus + "\n" + "Message: " + result.Message, LogLevels.Error, "Service", group.Owner);
                }
                result.IsSuccessful = serviceResult.IsSuccessful;
                result.Message = serviceResult.Message;
                return result;
            }
            catch (Exception exc)
            {
                _logger.Log("Error Message: " + exc.Message, LogLevels.Error, "Service", group.Owner, new UserOperation(Operations.ChoreList, 0));
                throw exc;
            }
        }

        //Returns list of incomplete chores by username in given group
        //incomplete task summary
        public async Task<ChoreResult> GetGroupIncompleteChores(GroupModel group)
        {
            try
            {
                var result = new ChoreResult();
                var choresByUser = new Dictionary<string, List<Chore>>();

                var groupMembersResult = await _groupManager.GetGroupMembers(group);
                if (groupMembersResult.IsSuccessful)
                {
                    var serviceResult = new ChoreResult();

                    //how is it retrieving the group members here?
                    

                    var groupMembers = groupMembersResult.ReturnedObject;
                    IEnumerable enumerable = groupMembers as IEnumerable;
                    // var usernames = (List<string>)emails.ReturnedObject;


                    var message = "";
                    foreach (UserProfile userProfile in enumerable)
                    {
                        var username = userProfile.Username;
                        Console.WriteLine(userProfile.Username);
                       // only recieving one back right now which is wecasacorp@gmail.com
                        var firstName = userProfile.FirstName;
                        var userAccount = new UserAccount(username);
                        serviceResult = _service.GetUserIncompleteChores(userAccount);


                        if (serviceResult.IsSuccessful)
                        {
                            var incompleteChores = (List<Chore>)serviceResult.ReturnedObject;
                            choresByUser.Add(username, incompleteChores);
                            if (incompleteChores.Count() > 0)
                            {
                                message += firstName + " has " + incompleteChores.Count.ToString() + " incomplete chores.\n";
                            }
                        }
                        else
                        {
                            result.IsSuccessful = serviceResult.IsSuccessful;
                            result.Message = serviceResult.Message;
                            _logger.Log("Group incomplete chores fetch error: " + result.ErrorStatus + "\n" + "Message: " + result.Message, LogLevels.Error, "Service", userAccount.Username);
                            return result;
                        }
                    }

                    // Prepare email
                    var from = "wecasacsulb@gmail.com";
                    var subject = "Incomplete Task Summary";
                    var rem = "Every Sunday";
                    var evnt = "Chore List";
                    if (message == "")
                    {
                        message = "Group has no incomplete chores.";
                    }

                   // Console.WriteLine(message);


                    // Sending email
                    foreach (UserProfile userProfile in enumerable)
                    {

                        var username = userProfile.Username;

                        var to = username;
                        Console.WriteLine("Sending to: " + to);
                        var response = NotificationService.ScheduleReminderEmail(from, to, subject, message, rem, evnt);
                        Console.WriteLine("Sent: " + to);


                    }

                    result.ReturnedObject = choresByUser;
                    result.IsSuccessful = serviceResult.IsSuccessful;
                    result.Message = serviceResult.Message;
                }
                else
                {
                    result.IsSuccessful = groupMembersResult.IsSuccessful;
                    result.Message = groupMembersResult.Message;
                    _logger.Log("Group members list fetch error: " + result.ErrorStatus + "\n" + "Message: " + result.Message, LogLevels.Error, "Service", group.Owner);
                }
                return result;
            }
            catch (Exception exc)
            {
                _logger.Log("Error Message: " + exc.Message, LogLevels.Error, "Service", group.Owner, new UserOperation(Operations.ChoreList, 0));
                throw exc;
            }
        }

        //Assignment Validation
        private async Task<ChoreResult> ReassignChore(Chore chore, List<String> newAssignments)
        {
            var result = new ChoreResult();

            if (newAssignments == null || newAssignments.Count == 0)
            {
                List<String> assignedToStr = new List<String>();
                assignedToStr.Add(chore.CreatedBy);
                chore.UsernamesAssignedTo = assignedToStr;
            }

            // Validate all users in UsernamesAssignedTo
            List<UserProfile> assignedTo = new List<UserProfile>();
            foreach (String username in chore.UsernamesAssignedTo)
            {
                // Check if user does not exist
                if (!_um.IsUsernameTaken(username))
                {
                    result.IsSuccessful = false;
                    result.Message = "Cannot assign chore to a user that does not exist.";
                    return result;
                }
                else // Populate AssignedTo with UserProfile (profile icons)
                {
                    var userProfileResult = await _um.GetUserProfile(new UserAccount(username));
                    if (userProfileResult.IsSuccessful)
                    {
                        assignedTo.Add((UserProfile)userProfileResult.ReturnedObject);
                    }
                    else
                    {
                        // Error fetching an assigned user's UserProfiles
                        result.IsSuccessful = false;
                        result.Message = userProfileResult.Message;
                        return result;
                    }
                }
            }
            result.IsSuccessful = true;
            result.Message = "Successfully fetched all assigned user's profiles.";
            result.ReturnedObject = assignedTo;
            return result;
        }

        // Used for chore with multiple assignments
        private bool NoDuplicateChores(List<Chore> chores, Chore chore)
        {
            //if a chore with choreId & choreDate doesnt already exist in choresPerDay[key] then
            for (var i = 0; i < chores.Count; i++)
            {
                var currChore = chores[i];
                var choreDate = (DateTime)chore.ChoreDate;
                var currChoreDate = (DateTime)currChore.ChoreDate;
                if (chore.ChoreId == currChore.ChoreId && choreDate.ToString("yyyy-MM-dd") == currChoreDate.ToString("yyyy-MM-dd") && choreDate.DayOfWeek == currChoreDate.DayOfWeek)
                {
                    return false;
                }
            }
            return true;
        }

    }
}

