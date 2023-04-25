using System;
using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;
using HAGSJP.WeCasa.Services.Implementations;
using MySqlX.XDevAPI.CRUD;
using MySqlX.XDevAPI.Common;
using System.Collections.Generic;
using System.Text;

namespace HAGSJP.WeCasa.Managers.Implementations
{
	public class ChoreManager
	{
        private readonly UserManager _um;
        private readonly ChoreService _service;
        private Logger _logger;
        private RemindersDAO remindersDAO;


        public ChoreManager()
		{
            _logger = new Logger(new AccountMariaDAO());
            _service = new ChoreService();
            _um = new UserManager();
            remindersDAO = new RemindersDAO();

        }

        public ChoreResult AddChore(Chore chore, UserAccount userAccount)
        {
            try
            {
                var result = new ChoreResult();

                chore.Created = DateTime.Now;
                chore.CreatedBy = userAccount.Username;
                chore.IsCompleted = false;

                var assignedProfilesRes = ReassignChore(chore, chore.UsernamesAssignedTo);
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
                    _logger.Log( "Add chore error: " + result.ErrorStatus  + "\n" +"Message: " + result.Message, LogLevels.Error, "Service", userAccount.Username);
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

        public ChoreResult EditChore(Chore chore, UserAccount userAccount)
        {
            try
            {
                var result = new ChoreResult();

                chore.LastUpdated = DateTime.Now;
                chore.LastUpdatedBy = userAccount.Username;
                chore.IsCompleted = false;

                var assignedProfilesRes = ReassignChore(chore, chore.UsernamesAssignedTo);
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

                chore.LastUpdated = DateTime.Now;
                chore.LastUpdatedBy = userAccount.Username;
                chore.IsCompleted = true;

                var serviceResult = _service.EditChore(chore);
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

        public ChoreResult UndoChore(Chore chore, UserAccount userAccount)
        {
            try
            {
                var result = new ChoreResult();

                chore.LastUpdated = DateTime.Now;
                chore.LastUpdatedBy = userAccount.Username;
                chore.IsCompleted = false;

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

        public ChoreResult GetGroupToDoChores(GroupModel group)
        {
            try
            {
                var result = new ChoreResult();

                var serviceResult = _service.GetGroupChores(group, 0);
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
                        foreach(String day in days)
                        {
                            choresPerDay[day].Add(chore);
                            Console.Write(chore);
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

                var serviceResult = _service.GetGroupChores(group, 1);
                if (serviceResult.IsSuccessful)
                {
                    Console.WriteLine("getting group chores success");
                    result.ReturnedObject = serviceResult.ReturnedObject;
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

        

        public ChoreResult GetUserToDoChores(UserAccount user)
        {
            try
            {
                var result = new ChoreResult();

                var serviceResult = _service.GetUserChores(user, 0);
                if (serviceResult.IsSuccessful)
                {
                    result.ReturnedObject = serviceResult.ReturnedObject;
                    _logger.Log("Group completed chores fetched successfully", LogLevels.Info, "Service", user.Username);
                }
                else
                {
                    _logger.Log("Chore fetch error: " + result.ErrorStatus + "\n" + "Message: " + result.Message, LogLevels.Error, "Service", user.Username);
                }
                result.IsSuccessful = serviceResult.IsSuccessful;
                result.Message = serviceResult.Message;
                return result;
            }
            catch (Exception exc)
            {
                _logger.Log("Error Message: " + exc.Message, LogLevels.Error, "Service", user.Username, new UserOperation(Operations.ChoreList, 0));
                throw exc;
            }
        }

        public ChoreResult GetUserCompletedChores(UserAccount user)
        {
            try
            {
                var result = new ChoreResult();

                var serviceResult = _service.GetUserChores(user, 1);
                if (serviceResult.IsSuccessful)
                {
                    result.ReturnedObject = serviceResult.ReturnedObject;
                    _logger.Log("User's completed chores fetched successfully", LogLevels.Info, "Service", user.Username);
                }
                else
                {
                    _logger.Log("Chore fetch error: " + result.ErrorStatus + "\n" + "Message: " + result.Message, LogLevels.Error, "Service", user.Username);
                }
                result.IsSuccessful = serviceResult.IsSuccessful;
                result.Message = serviceResult.Message;
                return result;
            }
            catch (Exception exc)
            {
                _logger.Log("Error Message: " + exc.Message, LogLevels.Error, "Service", user.Username, new UserOperation(Operations.ChoreList, 0));
                throw exc;
            }
        }

       /* public async Task<ChoreResult> IncompleteTaskSummary(GroupModel group, UserAccount user, Chore chore)
        {
            try
            {
                // Get the list of incomplete chores for the group
                var result = GetGroupToDoChores(group);
                if (!result.IsSuccessful)
                {
                    // If there was an error fetching the chores, return the result as-is
                    return result;
                }

                // Get the list of completed chores for the group
                var completedChores = GetGroupCompletedChores(group);
                if (!completedChores.IsSuccessful)
                {
                    // If there was an error fetching the completed chores, return the result as-is
                    return completedChores;
                }
                var result = GetGroupToDoChores(group);
                var incompleteChores = result.Where(c => !completedChores.ReturnedObject.Select(cc => cc.ChoreId).Contains(c.ChoreId)).ToList();
                // Filter the list of chores to only include incomplete ones
                if (incompleteChores.Count == 0)
                {
                    // If there are no incomplete chores, return a successful result with a message indicating so
                    return new ChoreResult
                    {
                        IsSuccessful = true,
                        Message = "There are no incomplete chores for group " + group.GroupName
                    };
                }
             
                // Generate the email message body
                var message = $"The following chores are incomplete for group {group.GroupName}: \n";
                foreach (var incompleteChore in incompleteChores)
                {
                    message += $"\n- {incompleteChore.Name}";
                }
                var from = "wecasacorporation@gmail.com";
                var subject = "Weekly Incomplete Task Summary";
                var rem = "immediately";
                var evnt = "Incomplete Task Summary";
                var emails = remindersDAO.GetGroupEmail(group);
                var usernames = (List<string>)emails.ReturnedObject;

                foreach (var username in usernames)
                {
                    var to = username;
                    var response = await NotificationService.ScheduleReminderEmail(from, to, subject, message, rem, evnt);
                }

                // Log success
                _logger.Log($"Incomplete chore reminder email sent successfully to all users in group {group.GroupName}", LogLevels.Info, "Service", user.Username);

                // Return a successful result with the email response
                return new ChoreResult
                {
                    IsSuccessful = true,
                    Message = $"Incomplete chore reminder email sent to all users in group {group.GroupName}",
                };
            }
            catch (Exception exc)
            {
                _logger.Log($"Error sending incomplete chore reminder email to all users in group {group.GroupName}: {exc.Message}", LogLevels.Error, "Service", user.Username, new UserOperation(Operations.ChoreList, 0));
                throw;
            }
        }*/





        //Assignment Validation
        private ChoreResult ReassignChore(Chore chore, List<String> newAssignments)
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
                    var userProfileResult = _um.GetUserProfile(new UserAccount(username));
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
    }
}

