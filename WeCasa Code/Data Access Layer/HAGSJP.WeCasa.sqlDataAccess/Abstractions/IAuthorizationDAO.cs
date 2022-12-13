using System;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Models.Security;

namespace HAGSJP.WeCasa.sqlDataAccess.Abstractions
{
    public interface IAuthorizationDAO
    {
        public AuthResult GetRole(UserAccount ua);
        public AuthResult GetClaims(UserAccount ua);
        public AuthResult GetActiveStatus(UserAccount ua);
        public AuthResult InsertClaims(UserAccount ua, List<Claim> newClaims);
    }
}

