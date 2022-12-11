using System;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Models.Security;

namespace HAGSJP.WeCasa.sqlDataAccess.Abstractions
{
	public interface IAuthorizationDAO
	{
		public ResultObj GetRole(UserAccount ua);
        public ResultObj GetClaims(UserAccount ua);
        public ResultObj GetActiveStatus(UserAccount ua);

    }
}

