using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;

namespace HAGSJP.WeCasa.Services.Implementations
{
    public class BulletinBoardService
    {
        private readonly BulletinBoardDAO _dao;
        private Logger _logger;

        public BulletinBoardService() 
        {
            _logger = new Logger(new AccountMariaDAO());
            _dao = new BulletinBoardDAO();
        }

        public List<Note> GetNotes(int groupId)
        {
           return (List<Note>)_dao.GetNotes(groupId).ReturnedObject;
        }

        public Result AddNote(Note note)
        {
            return ServiceTemplate(() => _dao.AddNote(note), "Add note", note.LastModifiedUser);
        }

        public Result DeleteNote(int noteId)
        {
           return ServiceTemplate(() => _dao.DeleteNote(noteId), "Delete note", noteId.ToString());
        }

        public Result UpdateNote(Note note)
        {
           return ServiceTemplate(() => _dao.UpdateNote(note), "Update note", note.LastModifiedUser);
        }

        public Result ServiceTemplate(Func<Result> serviceFunc, string feature, string username)
        {
            Result result = serviceFunc();
            try
            {
                if (!result.IsSuccessful)
                {
                    _logger.Log(feature + "failed: " + result.Message, LogLevels.Error, "Server", username);
                }
                return result;
            }
            catch(Exception exc)
            {
                _logger.Log(feature + "Error Message: " + exc.Message, LogLevels.Error, "Server", username, new UserOperation(Operations.BudgetBar,0));
                return result;
            }
        }
    }
}