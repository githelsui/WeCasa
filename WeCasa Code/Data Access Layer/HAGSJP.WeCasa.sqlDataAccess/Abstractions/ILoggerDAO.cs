using HAGSJP.WeCasa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HAGSJP.WeCasa.sqlDataAccess.Abstractions
{
    public interface ILoggerDAO
    {
        public Task<Result> LogData(string message);
    }
}
