using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HAGSJP.WeCasa.Logging.Abstractions
{
    public interface ILogger
    {
        Task<Result> Log(string message, LogLevels logLevel, string category, string username);
    }

    // Interface segregation principle
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
