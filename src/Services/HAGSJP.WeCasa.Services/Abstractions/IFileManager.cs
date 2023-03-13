using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using HAGSJP.WeCasa.Models.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace HAGSJP.WeCasa.Services.Implementations
{
    public interface IFileManager
    {
        public S3Result GetGroupFiles(string groupId);
        public S3Result GetDeletedFiles(string groupId);
        public S3Result UploadFile(IFormFile file, string groupId, string username);
        public S3Result DeleteFile(string fileName, string bucketName, string username);
    }
}