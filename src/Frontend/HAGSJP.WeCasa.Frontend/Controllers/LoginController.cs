using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HAGSJP.WeCasa.Services.Implementations;
using HAGSJP.WeCasa.Client;
using HAGSJP.WeCasa.Models;

namespace HAGSJP.WeCasa.Frontend.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : Controller
{

    [HttpPost]
    [Route("AttemptLogin")]
    public Result AttemptLogin([FromBody] LoginForm form)
    {
        RegistrationClient rc = new RegistrationClient();

        UserAccount ua = new UserAccount(form.Username, form.Password);

        Login login = new Login();
        Authentication auth = new Authentication();
        UserManager um = new UserManager();
        var result = login.AttemptLogin(ua);

        return result;
    }

    //TODO: POST request for OTP submission
    [HttpPost]
    public Result GetOTP([FromBody] LoginForm form)
    {
        RegistrationClient rc = new RegistrationClient();

        UserAccount ua = new UserAccount(form.Username, form.Password);

        Login login = new Login();
        Authentication auth = new Authentication();
        UserManager um = new UserManager();
        var result = login.LoginUser(ua, auth, um);

        return result;
    }
}