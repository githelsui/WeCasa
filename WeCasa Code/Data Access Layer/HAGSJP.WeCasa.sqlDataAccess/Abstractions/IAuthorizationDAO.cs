using System;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Models.Security;

namespace HAGSJP.WeCasa.sqlDataAccess.Abstractions
{
	public interface IAuthorizationDAO
	{
		public UserRoles GetRole(UserAccount ua);
        public Claims GetClaims(UserAccount ua);

    }
}

