using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HAGSJP.WeCasa.Services.Implementations;
using HAGSJP.WeCasa.Client;
using HAGSJP.WeCasa.Models;
using MySqlConnector;
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

    [HttpPost]
    [Route("GetGroups")]
    public GroupResult GetGroups([FromBody] LoginForm account)
    {
        UserAccount currentUser = new UserAccount(account.Username);
        GroupManager gm = new GroupManager();
        var result = gm.GetGroups(currentUser);
        return result;
    }

    [HttpPost]
    [Route("CreateGroup")]
    public GroupResult CreateGroup([FromBody] GroupForm form)
    {
        GroupModel group = new GroupModel();
        group.GroupId = form.GroupId;
        group.GroupName = form.GroupName;
        group.Owner = form.Owner;
        group.Icon = form.Icon;
        group.Features = form.Features;
        GroupManager gm = new GroupManager();
        return gm.CreateGroup(group);
    }

    [HttpPost]
    [Route("ValidateUser")]
    public Result ValidateUser([FromBody] LoginForm account)
    {
        var gm = new GroupManager();
        var result = gm.ValidateGroupMemberInvitation(account.Username);
        return result;
    }
}