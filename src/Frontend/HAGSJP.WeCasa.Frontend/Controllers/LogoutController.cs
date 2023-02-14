using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HAGSJP.WeCasa.Services.Implementations;
using HAGSJP.WeCasa.Client;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Models.Security;

namespace HAGSJP.WeCasa.Frontend.Controllers;

[ApiController]
[Route("[controller]")]
public class LogoutController : Controller
{

    [HttpPost]
    [Route("AttemptLogout")]
    public Result AttemptLogout()
    {
        UserAccount ua = new UserAccount();
        Logout logout = new Logout();
        return logout.LogoutUser(ua);
    }
}