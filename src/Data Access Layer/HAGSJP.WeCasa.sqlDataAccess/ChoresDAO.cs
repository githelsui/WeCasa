using System;
using HAGSJP.WeCasa.Models;
using MySqlConnector;

namespace HAGSJP.WeCasa.sqlDataAccess
{
	public class ChoresDAO : AccountMariaDAO
	{
        private string _connectionString;
        private DAOResult result;

        public MySqlConnectionStringBuilder BuildConnectionString()
        {
            var builder = new MySqlConnectionStringBuilder
            {
                Server = "localhost",
                Port = 3306,
                UserID = "HAGSJP.WeCasa.SqlUser",
                Password = "cecs491",
                Database = "HAGSJP.WeCasa"
            };
            return builder;
        }

        public DAOResult CreateChore()
		{
            throw new NotImplementedException();
		}

        public DAOResult UpdateChore()
        {
            throw new NotImplementedException();
        }

        public DAOResult GetChore()
        {
            throw new NotImplementedException();
        }
    }
}

