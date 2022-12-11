using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess.Abstractions;

namespace HAGSJP.WeCasa.Services.Implementations
{
    internal class Authorization : IAuthorization
    {
        private readonly IAuthorizationDAO _dao;

        public Authorization(IAuthorizationDAO dao)
        {
            _dao = dao;
        }

        public Result validateClaim(UserAccount ua)
        {
            var userClaims = _dao.GetClaims(ua);
            throw new NotImplementedException();
        }

        public Result validateAdminRole(UserAccount ua)
        {
            var userRole = _dao.GetRole(ua);
            throw new NotImplementedException();
        }
    }
}
