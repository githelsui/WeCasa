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
        public async Task<CalendarResult> GetEvents(GroupModel group)
        {
            var result = await _service.GetEvents(group);
            return result;
        }
        public async Task<Result> AddEvent(Event evnt)
        {
            var result = await _service.AddEvent(evnt);
            return result;
        }

    }
}