using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HAGSJP.WeCasa.Client;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Managers.Implementations;
using HAGSJP.WeCasa.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

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
        public async Task<CalendarResult> GetGroupEvents([FromBody] GroupForm groupForm)
        {
            var result = new CalendarResult();
            try
            {
                GroupModel group = new GroupModel(groupForm.GroupId);
                result = await _manager.GetEvents(group);
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
            DateTime eventDate = DateTime.ParseExact(eventForm.EventDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            try
            {
                Event e = new Event(eventForm.EventName, eventForm.Description, eventDate, eventForm.GroupId, eventForm.Repeats, eventForm.Type, eventForm.Reminder, eventForm.Color, eventForm.CreatedBy);
                result = await _manager.AddEvent(e);
            }
            catch (Exception ex)
            {
                result.IsSuccessful = false;
                result.Message = ex.Message;
            }
            return result;
        }

        [HttpPost]
        [Route("EditGroupEvent")]
        public async Task<Result> EditGroupEvent([FromBody] EventForm eventForm)
        {
            var result = new Result();
            DateTime eventDate = DateTime.ParseExact(eventForm.EventDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            try
            {
                Event e = new Event(eventForm.EventName, eventForm.Description, eventDate, eventForm.GroupId, eventForm.Repeats, eventForm.Type, eventForm.Reminder, eventForm.Color, eventForm.CreatedBy);
                result = await _manager.EditEvent(e);
            }
            catch (Exception ex)
            {
                result.IsSuccessful = false;
                result.Message = ex.Message;
            }
            return result;
        }

        [HttpPost]
        [Route("DeleteGroupEvent")]
        public async Task<Result> DeleteGroupEvent([FromBody] EventForm eventForm)
        {
            var result = new Result();
            DateTime eventDate = DateTime.ParseExact(eventForm.EventDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            try
            {
                Event e = new Event(eventForm.EventName, eventForm.Description, eventDate, eventForm.GroupId, eventForm.Repeats, eventForm.Type, eventForm.Reminder, eventForm.Color, eventForm.CreatedBy);
                result = await _manager.DeleteEvent(e);
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

