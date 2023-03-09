using HAGSJP.WeCasa.Logging.Implementations;
using HAGSJP.WeCasa.Models.Security;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Net;
using System;
using System.Collections;
using Microsoft.AspNetCore.Identity;

namespace HAGSJP.WeCasa.Services.Implementations
{
    public class FileManager : IFileManager
    {
        private readonly FilesS3DAO _dao;
        private Logger successLogger;
        private Logger errorLogger;

        public FileManager()
        {
            _dao = new FilesS3DAO();
        }
        public FileManager(FilesS3DAO dao)
        {
            _dao = dao;
        }

        public S3Result GetGroupFiles(string groupId)
        {
            var result = _dao.GetGroupFiles(groupId).Result;
            var files = result.Files;
            var filesArr = files.ToArray();
            result.ReturnedObject = filesArr;
            return result;
        }
        
    }
}