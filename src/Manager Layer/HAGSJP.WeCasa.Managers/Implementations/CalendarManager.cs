using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.Models.Security;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Services.Implementations;

namespace HAGSJP.WeCasa.Managers.Implementations
{
    public class CalendarManager : ICalendarManager
    {
        private CalendarService _service;

       public CalendarManager()
       {
            _service = new CalendarService();
       }
      
       public CalendarManager(CalendarService cs)
        {
            _service = cs;
        }
        public Result GetEvents(GroupModel group, DateTime date)
        {
            var result = _service.GetEvents(group, date);
            return result;
        }
        public Result AddEvent(Event evnt)
        {
            var result = _service.AddEvent(evnt);
            return result;
        }

    }
}