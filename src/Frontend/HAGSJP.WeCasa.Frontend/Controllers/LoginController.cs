using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HAGSJP.WeCasa.Services.Implementations;
using HAGSJP.WeCasa.Managers.Implementations;
using HAGSJP.WeCasa.Client;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Models.Security;

namespace HAGSJP.WeCasa.Frontend.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{

    [HttpPost]
    [Route("AttemptLogin")]
    public Result AttemptLogin([FromBody] LoginForm form)
    {
        UserAccount ua = new UserAccount(form.Username, form.Password);
        Login login = new Login();
        return login.AttemptLogin(ua);
    }

    [HttpPost]
    [Route("GetOTP")]
    public OTP GetOTP([FromBody] LoginForm form)
    {
        UserAccount ua = new UserAccount(form.Username);
        Login login = new Login();
        return login.GetOTP(ua);
    }

    [HttpPost]
    [Route("LoginWithOTP")]
    public Result LoginWithOTP([FromBody] LoginForm form)
    {
        UserAccount ua = new UserAccount(form.Username);
        Login login = new Login();
        return login.SubmitOTP(ua, form.Password);
    }

    [HttpPost]
    [Route("VerifyAdmin")]
    public Result VerifyAdmin([FromBody] LoginForm account)
    {
        try
        {
            var userManager = new UserManager();
            var result = userManager.ValidateAdminRole(new UserAccount(account.Username));
            if (result.IsSuccessful)
            {
                result.ErrorStatus = System.Net.HttpStatusCode.OK;
            }
            else
            {
                result.ErrorStatus = System.Net.HttpStatusCode.BadRequest;
            }
            return result;

        }
        catch (Exception exc)
        {
            return new ChoreResult(false, System.Net.HttpStatusCode.Conflict, exc.Message);
        }
    }
}