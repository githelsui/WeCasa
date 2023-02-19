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
public class HomeController : Controller
{
    [HttpPost]
    [Route("AttemptLogout")]
    public Result AttemptLogout([FromBody] LoginForm account)
    {
        UserAccount currentUser = new UserAccount(account.Username);
        Logout logout = new Logout();
        return logout.LogoutUser(currentUser);
    }

    [HttpGet]
    [Route("GetGroups")]
    public Result GetGroups([FromBody] LoginForm account)
    {
        UserAccount currentUser = new UserAccount(account.Username);
        GroupManager gm = new GroupManager();
        return gm.GetGroups(currentUser);
    }
}