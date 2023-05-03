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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace HAGSJP.WeCasa.Managers.Implementations
{
    public class FileManager : IFileManager
    {
        private readonly FilesS3DAO _dao;
        private Logger successLogger;
        private Logger errorLogger;

        public FileManager()
        {
            _dao = new FilesS3DAO();
            successLogger = new Logger(_dao);
            errorLogger = new Logger(_dao);
        }
        public FileManager(FilesS3DAO dao)
        {
            _dao = dao;
            successLogger = new Logger(dao);
            errorLogger = new Logger(dao);
        }

        public S3Result GetGroupFiles(string groupId)
        {
            var result = _dao.GetGroupFiles(groupId).Result;
            return result;
        }
        public S3Result GetDeletedFiles(string groupId)
        {
            var result = _dao.GetDeletedFiles(groupId).Result;
            return result;
        }

        public S3Result UploadFile(IFormFile file, string groupId, string username)
        {            
            var result = _dao.UploadFile(file, groupId, username).Result;
            if (result.IsSuccessful)
            {
                // Logging the file upload
                successLogger.Log("File uploaded successfully", LogLevels.Info, "Data Store", username, new UserOperation(Operations.FileUpload, 1));
            }
            else
            {
                // Logging the error
                errorLogger.Log("Error uploading a file", LogLevels.Error, "Data Store", username, new UserOperation(Operations.FileUpload, 0));
            }
            return result;
        }

        public S3Result DeleteFile(string fileName, string groupId, string username)
        {
            var result = _dao.DeleteFile(fileName, groupId, username).Result;
            if (result.IsSuccessful)
            {
                // Logging the file deletion
                successLogger.Log("File deleted successfully", LogLevels.Info, "Data Store", username, new UserOperation(Operations.FileDeletion, 1));
            }
            else
            {
                // Logging the error
                errorLogger.Log("Error deleting a file", LogLevels.Error, "Data Store", username, new UserOperation(Operations.FileDeletion, 0));
            }
            return result;
        }

    }
}