using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.sqlDataAccess;


namespace HAGSJP.WeCasa.Services.Implementations
{
    public class GroupManager
    {
        private readonly GroupDAO _dao;
        private Logger _logger;

        public GroupManager()
        {
            _dao = new GroupDAO();
            _logger = new Logger(_dao);
        }
 
    }
}