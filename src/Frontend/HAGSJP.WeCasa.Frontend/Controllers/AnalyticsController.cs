﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HAGSJP.WeCasa.Managers.Implementations;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HAGSJP.WeCasa.Frontend.Controllers
{
    [ApiController]
    [Route("analytics")]
    public class AnalyticsController : ControllerBase
    {
        public AnalyticsController()
        {
            //_manager = new ChoreManager();
            //_groupManager = new GroupManager();
        }

        [HttpPost]
        [Route("GetLoginsPerDay")]
        public KPIResult GetLoginsPerDay([FromBody] KPIForm kpiForm)
        {
            throw new NotImplementedException();
        }
    }
}

