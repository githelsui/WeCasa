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

    [HttpGet]
    public bool Get()
    {
        Console.Write("WORKS!!!");
        return true;
    }

    [HttpPost]
    public ViewResult Post([FromBody] UserAccount ua)
    {
        Console.Write("WEB API SERVER CONNECTED");
        ViewBag.Message = "test.";
        return View();
    }
}