using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Security.Principal;
using HAGSJP.WeCasa.Models.Security;

namespace HAGSJP.WeCasa.Services.Implementations
{
    public interface IAuthorization
    {
        public AuthResult ValidateClaim(UserAccount ua, Claim targetClaim);
        public AuthResult ValidateClaim(List<Claim> claims, Claim targetClaim);
        public AuthResult ValidateAdminRole(UserAccount ua);
        public AuthResult ValidateActiveUser(UserAccount ua);
        public AuthResult AddClaims(UserAccount ua, List<Claim> newClaims);
        public AuthResult AddClaims(UserAccount ua, Claim newClaim);
    }
}