using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.Models.Security;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Services.Implementations;
using Amazon.S3;
using HAGSJP.WeCasa.sqlDataAccess;
using System.Net;

namespace HAGSJP.WeCasa.Managers.Implementations
{
    public class CalendarManager : ICalendarManager
    {
        private CalendarService _service;
        private NotificationService _notificationService;
        private RemindersDAO remindersDAO;
        public bool isEmailSent;
        private Logger _log;

        public CalendarManager()
        {
            _service = new CalendarService();
            _notificationService = new NotificationService();
            remindersDAO = new RemindersDAO();
            var _logger = new Logger(new AccountMariaDAO());

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

        public async Task<Result> AddEvent(Event evnt)
        {
            Console.WriteLine("Entered AddEvent in manager");
            var result = _service.AddEvent(evnt);
            Console.WriteLine("Add event method went through");



            if (result.IsSuccessful && evnt.Reminder != null)
            {
                Console.WriteLine("preparing email");

                // Send notification for the new event
                Console.WriteLine("result is successfull and reminder is not null");
                var group = new GroupModel { GroupId = evnt.GroupId };
                var from = "wecasacsulb@gmail.com";
                var groupModel = new GroupModel
                {
                    GroupId = evnt.GroupId
                };

                // Get usernames from the group
                var rsult = remindersDAO.GetGroupEmail(groupModel);
                Console.WriteLine("DAO successfully got emails");

                // Get emails associated with each username
                var usernames = (List<string>)rsult.ReturnedObject;
                var eventDate = evnt.EventDate;
                string reminderOption = evnt.Reminder;
                DateTime sendDate;
                switch (reminderOption.ToLower())
                {
                    case "immediately":
                        sendDate = evnt.EventDate;
                        break;
                    case "30 minutes":
                        sendDate = evnt.EventDate.AddMinutes(-30);
                        break;
                    case "A day":
                        sendDate = evnt.EventDate.AddDays(-1);
                        break;
                    case "A week":
                        sendDate = evnt.EventDate.AddDays(-7);
                        break;
                    default:
                        throw new ArgumentException("Invalid reminder option");
                }



                string eventType = "event from calendar is coming up";
                string subject = "Reminder for " + eventType;
                string message = $"This is a reminder for " + eventType;

                foreach (var username in usernames)
                {
                    try
                    {
                        string to = username;
                        var response = await NotificationService.ScheduleReminderEmail(from, to, subject, message, reminderOption, eventType);
                        if (response == true)
                        {
                            _log.Log("Success sending email from Calendar", LogLevels.Info, "Data Store", to);

                        }
                        else
                        {
                            // Log error if email was not sent successfully
                            _log.Log("Error sending email from Calendar", LogLevels.Error, "Data Store", to);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log error
                        _log.Log("Error sending email from Calendar", LogLevels.Error, "Data Store", username);
                        throw;

                    }
                }
            }

            return result;

        }

    }
}
    

