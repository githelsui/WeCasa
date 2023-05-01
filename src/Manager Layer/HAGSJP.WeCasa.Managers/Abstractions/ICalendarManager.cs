using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using HAGSJP.WeCasa.Models.Security;

namespace HAGSJP.WeCasa.Managers.Implementations
{
    public interface ICalendarManager
    {
        public Task<DAOResult> GetEvents(GroupModel group);
        public Task<Result> AddEvent(Event e);
    }
}