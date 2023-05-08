using System;
using System.Text.RegularExpressions;
using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;
using MySqlConnector;
using MySqlX.XDevAPI.Common;

namespace HAGSJP.WeCasa.Services.Implementations
{
	public class CalendarService
	{
        private readonly CalendarDAO _dao;
        private Logger successLogger;
        private Logger errorLogger;

        public CalendarService()
		{
            _dao = new CalendarDAO();
            successLogger = new Logger(_dao);
            errorLogger = new Logger(_dao);
        }

        public async Task<CalendarResult> GetEvents(GroupModel group)
        {
            var getEventsResult = await _dao.GetEvents(group.GroupId);
            if (!getEventsResult.IsSuccessful)
            {
                await errorLogger.Log("Error getting calendar events", LogLevels.Error, "Data Store", group.GroupId.ToString());
            }

            return getEventsResult;
        }
        public async Task<DAOResult> AddEvent(Event evnt)
        {
            var addEventResult = await _dao.AddEvent(evnt);
            if (addEventResult.IsSuccessful)
            {
                addEventResult.Message = "Event created successfully";
                await successLogger.Log(addEventResult.Message, LogLevels.Info, "Data Store", evnt.GroupId.ToString());
            }
            else
            {
                await errorLogger.Log("Error creating an event", LogLevels.Error, "Data Store", evnt.GroupId.ToString());
            }

            return addEventResult;
        }
        public async Task<DAOResult> EditEvent(Event evnt)
        {
            var editEventResult = await _dao.UpdateEvent(evnt);
            if (editEventResult.IsSuccessful)
            {
                editEventResult.Message = "Event updated successfully";
                await successLogger.Log(editEventResult.Message, LogLevels.Info, "Data Store", evnt.GroupId.ToString());
            }
            else
            {
                await errorLogger.Log("Error updating an event", LogLevels.Error, "Data Store", evnt.GroupId.ToString());
            }

            return editEventResult;
        }
        public async Task<DAOResult> DeleteEvent(Event evnt)
        {
            var deleteEventResult = await _dao.DeleteEvent(evnt);
            if (deleteEventResult.IsSuccessful)
            {
                deleteEventResult.Message = "Event deleted successfully";
            }

            return deleteEventResult;
        }
    }
}

