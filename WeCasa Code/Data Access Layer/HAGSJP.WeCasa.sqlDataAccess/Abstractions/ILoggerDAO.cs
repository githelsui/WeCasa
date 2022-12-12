using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Models.Security;
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
        public Task<Result> LogData(Log log);
    }
}
