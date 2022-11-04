using HAGSJP.WeCasa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HAGSJP.WeCasa.Logging.Abstractions
{
    public interface ILogger
    {
        Result Log(string message);
    }

    // interface segregation principle
    public interface IDataAccessObject
    {
        Result ReadData();
        Result WriteData();
    }

    public interface IDataReader
    {
        Result ReadData();
    }

    public interface IDataWriter
    {
        Result WriteData(string data);
    }
}
