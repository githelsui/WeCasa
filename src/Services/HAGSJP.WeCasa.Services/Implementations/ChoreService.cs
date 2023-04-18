using System;
using HAGSJP.WeCasa.Logging.Implementations;
using System.Data.SqlTypes;
using System.Text;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;
using HAGSJP.WeCasa.sqlDataAccess.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace HAGSJP.WeCasa.Services.Implementations
{
    public class ChoreService
    {
        private readonly GroupMariaDAO _dao;
        private Logger successLogger;
        private Logger errorLogger;
        public ChoreService()
        {
            _dao = new GroupMariaDAO();
            successLogger = new Logger(_dao);
            errorLogger = new Logger(_dao);
        }
        public ChoreService(GroupMariaDAO dao)
        {
            _dao = dao;
            successLogger = new Logger(dao);
            errorLogger = new Logger(dao);
        }
    }
}
