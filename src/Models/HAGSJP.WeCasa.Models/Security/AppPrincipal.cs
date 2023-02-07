using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

namespace HAGSJP.WeCasa.Models.Security
{
    public class AppPrincipal : ClaimsPrincipal
    {
        public string GetRoleName()
        {
            return "";
        }
    }
}
