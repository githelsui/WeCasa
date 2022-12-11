using HAGSJP.WeCasa.Models.Security;
using HAGSJP.WeCasa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HAGSJP.WeCasa.Services.Implementations
{
    public class Authentication : IAuthentication
    {
        // Ensuring that the one-time code has not expired
        public Result VerifyOTPassword(OTP otp)
        {
            throw new NotImplementedException();
        }

    }
}
