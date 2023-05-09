using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;
using HAGSJP.WeCasa.Services.Implementations;
using Microsoft.AspNetCore.Http;

namespace HAGSJP.WeCasa.Managers.Implementations
{
    public class BulletinBoardManager
    {
        private readonly BulletinBoardService _bulletinBoardService;
        private Logger _logger;
        private RemindersDAO remindersDAO;
        private Result response;
        private readonly FilesS3DAO _fileS3DAO; 

        public BulletinBoardManager() 
        {
            _logger = new Logger(new AccountMariaDAO());
            _bulletinBoardService = new BulletinBoardService();
            remindersDAO = new RemindersDAO();

            _fileS3DAO = new FilesS3DAO();
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
                    NotificationService service = new NotificationService();
                    var response = service.ScheduleReminderEmail(from, to, subject, message, rem, evnt);
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

        public S3Result UploadFile(IFormFile file, string groupId, string username)
        {           
            try
            {  
                string path = "bulletin-board/" + username;
                var result = _fileS3DAO.UploadFile(file, groupId, path).Result;
                if (result.IsSuccessful)
                {
                    // Logging the file upload
                    _logger.Log("File uploaded successfully", LogLevels.Info, "Data Store", username, new UserOperation(Operations.FileUpload, 1));
                }
                else
                {
                    // Logging the error
                    _logger.Log("Error uploading a file", LogLevels.Error, "Data Store", username, new UserOperation(Operations.FileUpload, 0));
                }
                return result;
            }
            catch(Exception ex) 
            {
                _logger.Log("Error: " + ex.Message, LogLevels.Error, "Data Store", username, new UserOperation(Operations.FileUpload, 0));
                return new S3Result(false, System.Net.HttpStatusCode.InternalServerError, "File Upload Fail");
            }
        }

        public S3Result DeleteFile(string filePath, string groupId, string username)
        {
            string path = "bulletin-board/" + filePath;
            var result = _fileS3DAO.DeleteFile(path, groupId, "").Result;
            if (result.IsSuccessful)
            {
                // Logging the file deletion
                _logger.Log("File deleted successfully", LogLevels.Info, "Data Store", username, new UserOperation(Operations.FileDeletion, 1));
            }
            else
            {
                // Logging the error
                _logger.Log("Error deleting a file", LogLevels.Error, "Data Store", username, new UserOperation(Operations.FileDeletion, 0));
            }
            return result;
        }
    }
}