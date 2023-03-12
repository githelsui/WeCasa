using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HAGSJP.WeCasa.Client;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Services.Implementations;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HAGSJP.WeCasa.Frontend.Controllers
{
    [ApiController]
    [Route("files")]
    public class FileController : Controller
    {
        [HttpGet]
        [Route("GetGroupFiles")]
        public S3Result GetGroupFiles()
        {
            FileManager fm = new FileManager();
            var result = fm.GetGroupFiles("");
            return result;
        }

        [HttpPost]
        [Route("DeleteFile")]
        public S3Result DeleteFile([FromBody] FileForm fileForm)
        {
            FileManager fm = new FileManager();
            return fm.DeleteFile(fileForm.FileName, "");
        }

        [HttpPost]
        [Route("DownloadFile")]
        public S3Result DownloadFile([FromBody] FileForm fileForm)
        {
            FileManager fm = new FileManager();
            throw new NotImplementedException();
        }
    }
}

