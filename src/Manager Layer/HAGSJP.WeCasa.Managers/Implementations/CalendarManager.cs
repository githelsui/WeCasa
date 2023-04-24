using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.Models.Security;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Services.Implementations;
using Amazon.S3;
using HAGSJP.WeCasa.sqlDataAccess;

namespace HAGSJP.WeCasa.Managers.Implementations
{
    public class CalendarManager : ICalendarManager
    {
        private CalendarService _service;
        private NotificationService _notificationService;
        private RemindersDAO remindersDAO;

        public CalendarManager()
        {
            _service = new CalendarService();
            _notificationService = new NotificationService();
            remindersDAO = new RemindersDAO();
        }

        public CalendarManager(CalendarService cs, NotificationService ns)
        {
            _service = cs;
            _notificationService = ns;
        }

        public Result GetEvents(GroupModel group, DateTime date)
        {
            var result = _service.GetEvents(group, date);
            return result;
        }

        public async Task<Result> AddEventAsync(Event evnt)
        {
            var result = _service.AddEvent(evnt);

            if (result.IsSuccessful & evnt.Reminder != null)
            {
                // Send notification for the new event
                var group = new GroupModel { GroupId = evnt.GroupId };
                var from = "wecasacorporation@gmail.com";
                var groupModel = new GroupModel
                {
                    GroupId = evnt.GroupId
                };

                // Get usernames from the group
                var rsult = remindersDAO.GetGroupEmail(groupModel);

                // Get emails associated with each username
                var usernames = (List<string>)rsult.ReturnedObject;
                var eventDate = evnt.EventDate;
                string reminderOption = evnt.Reminder;
                string eventType = "event from calendar is coming up";
                string subject = "Reminder for" + eventType;
                string message = $"This is a reminder for" + eventType;
                foreach (var username in usernames)
                {
                        string to = username;
                        await NotificationService.ScheduleReminderEmail(from, to, subject, message, reminderOption, eventType);

                    }
                }
                return result;
        }
    }
}
