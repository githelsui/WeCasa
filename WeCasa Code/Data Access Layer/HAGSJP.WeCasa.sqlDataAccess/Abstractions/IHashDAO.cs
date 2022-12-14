using System;
using HAGSJP.WeCasa.Models;

namespace HAGSJP.WeCasa.sqlDataAccess.Abstractions
{
	public interface IHashDAO
	{
        public AuthResult GetEncryptedPassword(string username);
        public AuthResult GetSalt(string username);
        public AuthResult SaveSalt(string username);
    }
}

