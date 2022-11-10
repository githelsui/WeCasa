using HAGSJP.WeCasa.Models;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HAGSJP.WeCasa.sqlDataAccess.Abstractions
{
    public interface ILoggerDAO
    {
        public Result AddUser(User user, string password);
        public Task<Result> LogData(Log log);
    }
}
