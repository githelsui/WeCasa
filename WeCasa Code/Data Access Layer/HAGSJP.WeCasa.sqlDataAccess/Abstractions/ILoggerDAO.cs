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
        public Result AddUser(string email, string username, string password);
        public Task<Result> LogData(string message, string logLevel, string category, DateTime dateTime, int userId);
    }
}
