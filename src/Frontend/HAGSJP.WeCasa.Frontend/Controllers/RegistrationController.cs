using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HAGSJP.WeCasa.Client;
using HAGSJP.WeCasa.Models;

namespace HAGSJP.WeCasa.Frontend.Controllers;

[ApiController]
[Route("[controller]")]
public class RegistrationController : Controller
{
    [HttpPost]
    public Result Post([FromBody]RegistrationForm form)
    {
        RegistrationClient rc = new RegistrationClient();

        var result = rc.Register(form.FirstName, form.LastName, form.Username, form.Password);

        return result;
    }
}