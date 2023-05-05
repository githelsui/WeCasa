using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;
using HAGSJP.WeCasa.Services.Implementations;

namespace HAGSJP.WeCasa.Managers.Implementations
{
    public class BulletinBoardManager
    {
        private readonly BulletinBoardService _bulletinBoardService;
        private Logger _logger;
        private RemindersDAO remindersDAO;
        private Result response;

        public BulletinBoardManager() 
        {
            _logger = new Logger(new AccountMariaDAO());
            _bulletinBoardService = new BulletinBoardService();
            remindersDAO = new RemindersDAO();

        }

        public List<Note> GetNotes(int groupId)
        {
            return _bulletinBoardService.GetNotes(groupId);
        }

        public Result AddNote(Note note)
        {
            var result = _bulletinBoardService.AddNote(note);
            var groupmod = new GroupModel { GroupId = note.GroupId };

            if (result.IsSuccessful)
            {
                var group = new GroupModel { GroupId = note.GroupId };
                var emails = remindersDAO.GetGroupEmail(groupmod);
                var usernames = (List<string>)emails.ReturnedObject;
                var from = "wecasacsulb@gmail.com";
                var subject = "New note added to the bulletin";
                var message = note.Message;
                var rem = "immediately";
                var evnt = "New note added to the bulletin";

                foreach (var username in usernames)
                {
                    var to = username;
                    Console.WriteLine("Sending to: " + to);
                    var response = NotificationService.ScheduleReminderEmail(from, to, subject, message, rem, evnt);
                    Console.WriteLine("Sent to: " + to);

                }

            }
            return ManagerTemplate(() => _bulletinBoardService.AddNote(note), "Add note", note.LastModifiedUser);
        }

        public Result DeleteNote(int noteId)
        {
           return ManagerTemplate(() => _bulletinBoardService.DeleteNote(noteId), "Delete note", noteId.ToString());
        }

        public Result UpdateNote(Note note)
        {
            return ManagerTemplate(() => _bulletinBoardService.UpdateNote(note), "Update note", note.LastModifiedUser);
        }

        public Result ManagerTemplate(Func<Result> serviceFunc, string feature, string username)
        {
            Result result = serviceFunc();
            try
            {
                if (!result.IsSuccessful)
                {
                    _logger.Log(feature + " was not successful", LogLevels.Error, "Business", username);
                }
                return result;
            }
            catch(Exception exc)
            {
                _logger.Log(feature + "Error Message: " + exc.Message, LogLevels.Error, "Business", username, new UserOperation(Operations.BudgetBar,0));
                return result;
            }
        }
    }
}