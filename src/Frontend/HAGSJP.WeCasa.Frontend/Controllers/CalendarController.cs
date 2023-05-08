using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HAGSJP.WeCasa.Client;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Managers.Implementations;
using HAGSJP.WeCasa.Services.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace HAGSJP.WeCasa.Frontend.Controllers
{
    [ApiController]
    [Route("calendar")]
    public class CalendarController : ControllerBase
    {
        private readonly CalendarManager _manager;
        public CalendarController()
        {
            _manager = new CalendarManager();
        }

        [HttpPost]
        [Route("GetGroupEvents")]
        public async Task<Result> GetGroupEvents([FromQuery] int groupId)
        {
            var result = new Result();
            DateTime date = new DateTime();
            try
            {
                GroupModel group = new GroupModel(groupId);
                result = _manager.GetEvents(group, date);
            }
            catch (Exception ex)
            {
                result.IsSuccessful = false;
                result.Message = ex.Message;
            }
            return result;
        }

        [HttpPost]
        [Route("AddGroupEvent")]
        public async Task<Result> AddGroupEvent([FromBody] EventForm eventForm)
        {
            var result = new Result();
            try
            {
                Event e = new Event(eventForm.EventName, eventForm.Description, eventForm.EventDate, eventForm.GroupId, eventForm.Repeats, eventForm.Type, eventForm.Reminder, eventForm.Color, eventForm.CreatedBy);
                result = await _manager.AddEvent(e);
            }
            catch (Exception ex)
            {
                result.IsSuccessful = false;
                result.Message = ex.Message;
            }
            return result;
        }
    }
}

