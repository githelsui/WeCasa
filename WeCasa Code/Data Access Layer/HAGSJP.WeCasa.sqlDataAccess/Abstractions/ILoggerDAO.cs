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
        public Result LogData(string message)
        {
            return new Result(); // returns Noop (No operation)
        }
    }
}
