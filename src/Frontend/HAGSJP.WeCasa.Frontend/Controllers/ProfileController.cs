using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HAGSJP.WeCasa.Services.Implementations;
using HAGSJP.WeCasa.Client;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Models.Security;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HAGSJP.WeCasa.Frontend.Controllers
{
    [ApiController]
    [Route("edit-profile")]
    public class ProfileController : Controller
    {
        [HttpPost]
        [Route("UpdateFullName")]
        public Result UpdateFullName([FromBody] ProfileForm form)
        {
            var result = new Result();
            UserManager um = new UserManager();
            UserAccount user = new UserAccount(form.Email);
            result = um.UpdateFirstName(user, form.FirstName);
            if (result.IsSuccessful)
            {
                result = um.UpdateLastName(user, form.LastName);
            }
            return result;
        }

        [HttpPost]
        [Route("UpdateProfileIcon")]
        public Result UpdateProfileIcon([FromBody] ProfileForm form)
        {
            UserManager um = new UserManager();
            UserAccount user = new UserAccount(form.Email);
            var result = um.UpdateUserIcon(user, form.Image.Value);
            return result;
        }

        [HttpPost]
        [Route("GetUserProfile")]
        public Result GetUserProfile([FromBody] ProfileForm form)
        {
            UserManager um = new UserManager();
            UserAccount user = new UserAccount(form.Email);
            var result = um.GetUserProfile(user);
            return result;
        }
    }
}

