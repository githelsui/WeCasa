using System;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess.Abstractions;
using MySqlConnector;

namespace HAGSJP.WeCasa.sqlDataAccess
{
	public class HashDAO : IHashDAO
	{
        private string _connectionString;

        public HashDAO() {}

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

        public Result ValidateSqlStatement(int rows)
        {
            var result = new Result();

            if (rows == 1)
            {
                result.IsSuccessful = true;
                result.Message = string.Empty;

                return result;
            }
            result.IsSuccessful = false;
            result.Message = $"Rows affected were not 1. It was {rows}";

            return result;
        }

        public AuthResult PushEncryptedPassword(UserAccount ua)
        {
            throw new NotImplementedException();
        }

        public AuthResult GetEncryptedPassword(string username)
        {
            throw new NotImplementedException();
        }

        public AuthResult GetSalt(string username)
        {
            throw new NotImplementedException();
        }

        public AuthResult SaveSalt(string username)
        {
            throw new NotImplementedException();
        }
    }
}

