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

namespace HAGSJP.WeCasa.Managers.Implementations
{
	public class ChoreManager
	{
        private readonly UserManager _um;
        private readonly ChoreService _service;
        private Logger _logger;

        public ChoreManager()
		{
            _logger = new Logger(new AccountMariaDAO());
            _service = new ChoreService();
            _um = new UserManager();
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
                        var isCompleted = (bool)chore.IsCompleted;

                        //Account for Repeats property
                        if (!string.IsNullOrEmpty(chore.Repeats))
                        {
                                var creationDate = (DateTime)chore.Created;
                                var completedDate = (chore.LastCompleted != null) ? (DateTime)chore.LastCompleted : creationDate;
                                TimeSpan timeSpan = currentDate - completedDate;

                            // check if currentDate is within the same week as CreatedBy property -> chore was created for that week
                            if (WithinSameWeek(currentDate, creationDate) && !isCompleted)
                            {
                                foreach (String day in days)
                                {
                                    choresPerDay[day].Add(chore);
                                    Console.Write(chore);
                                }
                            }
                            else
                            {
                                if (chore.Repeats == "Monthly")
                                {
                                    // only add to currentToDo if 1 month has passed since last updated / completed
                                    if ((currentDate.Month - completedDate.Month) == 1 && (currentDate.Year == completedDate.Year || currentDate.Year == completedDate.Year + 1))
                                    {
                                        if (!(WithinSameWeek(currentDate, completedDate) && isCompleted))
                                        {
                                            foreach (String day in days)
                                            {
                                                choresPerDay[day].Add(chore);
                                                Console.Write(chore);
                                            }
                                        }
                                    }
                                }

                                if (chore.Repeats == "Bi-weekly")
                                {
                                    int weeks = (int)(timeSpan.TotalDays / 7) + 1; // +1 in order to not count the current week
                                    if (weeks % 2 == 0) // Check if two weeks has passed since last completion
                                    {
                                        //chore is already completed for this specific week
                                        //!(true && true) -> false || false
                                        if (!(WithinSameWeek(currentDate, completedDate) && isCompleted))
                                        {
                                            foreach (String day in days)
                                            {
                                                choresPerDay[day].Add(chore);
                                                Console.Write(chore);
                                            }
                                        }
                                    }
                                }

                                if (chore.Repeats == "Weekly")
                                {
                                    if (!(WithinSameWeek(currentDate, completedDate) && isCompleted))
                                    {
                                        foreach (String day in days)
                                        {
                                            choresPerDay[day].Add(chore);
                                            Console.Write(chore);
                                        }
                                    }
                                }
                            }
                        }
                        else //Chore only occurs during that week it was created for 
                        {
                            if (isCompleted == false)
                            {
                                foreach (String day in days)
                                {
                                    choresPerDay[day].Add(chore);
                                    Console.Write(chore);
                                }
                            }
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
                    List<Chore> resultQuery = (List<Chore>)serviceResult.ReturnedObject;
                    var choresPerDay = new Dictionary<string, List<Chore>>();
                    for (var i = 0; i < resultQuery.Count; i++)
                    {
                        var chore = resultQuery[i];
                        var dateCompleted = chore.LastUpdated;
                        string key = string.Format("{0:dddd MM/dd/yy}", dateCompleted);
                        if(choresPerDay.ContainsKey(key))
                        {
                            choresPerDay[key].Add(chore);
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

        private bool WithinSameWeek(DateTime currrentDate, DateTime otherDate)
        {
            CultureInfo culture = CultureInfo.CurrentCulture;
            Calendar calendar = culture.Calendar;
            int week1 = calendar.GetWeekOfYear(currrentDate, culture.DateTimeFormat.CalendarWeekRule, culture.DateTimeFormat.FirstDayOfWeek);
            int week2 = calendar.GetWeekOfYear(otherDate, culture.DateTimeFormat.CalendarWeekRule, culture.DateTimeFormat.FirstDayOfWeek);

            if (week1 == week2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

