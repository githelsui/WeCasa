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
        public S3Result GetGroupFiles([FromQuery] string groupId)
        {
            FileManager fm = new FileManager();
            var result = fm.GetGroupFiles(groupId);
            return result;
        }

        [HttpPost]
        [Route("UploadFile")]
        public S3Result UploadFile([FromForm] FileForm fileForm)
        {
            FileManager fm = new FileManager();
            var file = fileForm.File;
            var groupId = fileForm.GroupId;
            var owner = fileForm.Owner;
            var result = fm.UploadFile(file, groupId, owner);
            return result;
        }

        [HttpPost]
        [Route("DeleteFile")]
        public S3Result DeleteFile([FromBody] FileForm fileForm)
        {
            FileManager fm = new FileManager();
            var fileName = fileForm.Owner + '/' + fileForm.FileName;
            var groupId = fileForm.GroupId;
            var owner = fileForm.Owner;
            return fm.DeleteFile(fileName, groupId, owner);
        }
    }
}

