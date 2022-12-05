using System;
using HAGSJP.WeCasa.Models;

namespace HAGSJP.WeCasa.sqlDataAccess.Abstractions
{
	public interface IAuthorizationDAO
	{
		public UserRoles getRole(string email);
	}
}

