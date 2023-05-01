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

        public async Task<DAOResult> GetEvents(GroupModel group)
        {
            var getEventsResult = await _dao.GetEvents(group.GroupId);
            if (getEventsResult.IsSuccessful)
            {
                await successLogger.Log("Successfully retrieved calendar events", LogLevels.Info, "Data Store", group.GroupId.ToString());
            }
            else
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
                await successLogger.Log("Event created successfully", LogLevels.Info, "Data Store", evnt.GroupId.ToString());
            }
            else
            {
                await errorLogger.Log("Error creating an event", LogLevels.Error, "Data Store", evnt.GroupId.ToString());
            }

            return addEventResult;
        }
    }
}

