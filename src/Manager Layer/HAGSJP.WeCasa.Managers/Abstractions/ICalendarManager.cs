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
        public Task<CalendarResult> GetEvents(GroupModel group);
        public Task<Result> AddEvent(Event e);
        public Task<Result> EditEvent(Event e);
        public Task<Result> DeleteEvent(Event e);
    }
}