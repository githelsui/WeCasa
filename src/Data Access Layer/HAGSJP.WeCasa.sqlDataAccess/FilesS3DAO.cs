using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Azure;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.sqlDataAccess.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Client;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace HAGSJP.WeCasa.sqlDataAccess
{
    public class FilesS3DAO : ILoggerDAO
    {
        // Configuring AWS S3 client for hagsjp.wecasa.s3 user
        private AmazonS3Client _client = new AmazonS3Client(
            "AKIA2K6ZUAG7SOZBM5KV",
            "",
            Amazon.RegionEndpoint.USEast2
        );
        private string _bucketName = "wecasa-group-files-";

        public FilesS3DAO() { }
        
        public string ConvertS3ObjectSize(long bytes)
        {
            string[] units = { "B", "KB", "MB", "GB", "TB" };
            int index = 0;
            double size = (double)bytes;
            while(size >= 1024 && index < units.Length - 1)
            {
                index++;
                size /= 1024;
            }

            return $"{Math.Round(size, 2)} {units[index]}";
        }

        public async Task<S3Result> CreateBucket(string groupId)
        {
            var result = new S3Result();
            var bucketName = _bucketName + groupId;
             try
            {
                var response = await _client.PutBucketAsync(bucketName);
                // Enabling bucket versioning
                var versioningRequest = new PutBucketVersioningRequest
                {
                    BucketName = bucketName,
                    VersioningConfig = new S3BucketVersioningConfig
                    {
                        Status = VersionStatus.Enabled
                    }
                };
                var versioningResponse = await _client.PutBucketVersioningAsync(versioningRequest);
                result.Message = $"{bucketName} bucket successfully created.";
                result.ErrorStatus = response.HttpStatusCode;
            } 
            catch(AmazonS3Exception ex)
            {
                result.ErrorStatus = ex.StatusCode;
                result.Message = ex.Message;
            }
            return result; 
        }

        public async Task<S3Result> DeleteAllObjects(string groupId)
        {
            var result = new S3Result();
            var bucketName = _bucketName + groupId;
            var request = new ListObjectsV2Request { BucketName = _bucketName + groupId };
            ListObjectsV2Response response;

            do
            {
                response = await _client.ListObjectsV2Async(request);
                foreach (var s3Obj in response.S3Objects)
                {
                    
                    await _client.DeleteObjectAsync(bucketName, s3Obj.Key);
                }
                request.ContinuationToken = response.NextContinuationToken;
            } while (response.IsTruncated);

            result.ErrorStatus = response.HttpStatusCode;
            result.IsSuccessful = true;
            return result;
        }

        public async Task<S3Result> DeleteBucket(string groupId)
        {
            var result = new S3Result();
            var bucketName = _bucketName + groupId;
            try
            {
                var response = await _client.DeleteBucketAsync(bucketName);
                result.Message = $"{bucketName} bucket successfully deleted.";
                result.ErrorStatus = response.HttpStatusCode;
            }
            catch (AmazonS3Exception ex)
            {
                result.ErrorStatus = ex.StatusCode;
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<S3Result> GetGroupFiles(string groupId)
        {
            var result = new S3Result();
            var s3Objects = new List<S3ObjectModel>();
            var request = new ListObjectsV2Request { BucketName = _bucketName + groupId };
            ListObjectsV2Response response;

            do
            {
                response = await _client.ListObjectsV2Async(request);
                foreach (var s3Obj in response.S3Objects)
                {
                    var getObjectRequest = new GetObjectRequest
                    {
                        BucketName = _bucketName+groupId, 
                        Key = s3Obj.Key
                    };
                    var getDataResponse = await _client.GetObjectAsync(getObjectRequest);

                    using (var memoryStream = new MemoryStream())
                    {
                        await getDataResponse.ResponseStream.CopyToAsync(memoryStream);
                        var s3ObjData = memoryStream.ToArray();
                        var fileExtension = Path.GetExtension(s3Obj.Key);
                        s3Objects.Add(new S3ObjectModel(
                            s3Obj.Key.ToString(),
                            s3ObjData,
                            fileExtension,
                            ConvertS3ObjectSize(s3Obj.Size),
                            s3Obj.LastModified
                        ));
                    }

                }
                request.ContinuationToken = response.NextContinuationToken;
            } while (response.IsTruncated);

            result.ReturnedObject = s3Objects;
            result.ErrorStatus = response.HttpStatusCode;
            result.Message = $"Number of group files retrieved: {response.KeyCount}";
            result.IsSuccessful = true;
            return result;
        }

        public async Task<S3Result> UploadFile(IFormFile file, string groupId, string? prefix)
        {
            var result = new S3Result();
            var request = new PutObjectRequest()
            {
                BucketName = _bucketName+groupId,
                Key = string.IsNullOrEmpty(prefix) ? file.FileName : $"{prefix?.TrimEnd('/')}/{file.FileName}",
                InputStream = file.OpenReadStream()
            };
            
            var response = await _client.PutObjectAsync(request);
            result.ErrorStatus = response.HttpStatusCode;
            result.Message = $"{file.FileName} successfully uploaded.";
            result.IsSuccessful = true;
            return result;
        }

        public async Task<S3Result> DeleteFile(string fileName, string groupId, string? prefix)
        {
            var result = new S3Result();
            var bucketName = _bucketName + groupId;
            var request = new DeleteObjectRequest()
            {
                BucketName = bucketName,
                Key = fileName
            };
            try
            {
                var response = await _client.DeleteObjectAsync(request);
                result.ErrorStatus = response.HttpStatusCode;
                result.Message = $"{fileName} successfully deleted from {bucketName}.";
                result.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                result.Message = $"Error deleting {fileName}: {ex.Message}";
                result.IsSuccessful = false;
            }
            return result;
        }

        public async Task<Result> LogData(Log log)
        {
            var result = new Result();
            var connectionString = new MySqlConnectionStringBuilder
             {
                Server = "localhost",
                 Port = 3306,
                 UserID = "HAGSJP.WeCasa.SqlUser",
                 Password = "cecs491",
                 Database = "HAGSJP.WeCasa"
             }.ConnectionString;

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                // Insert SQL statement
                var insertSql = @"INSERT INTO `Logs` 
                                    (`Message`, `Log_Level`, `Category`, `Username`, `Operation`, `Success`) 
                                VALUES 
                                    (@message, @logLevel, @category, @username, @operation, @success);";

                var command = connection.CreateCommand();
                command.CommandText = insertSql;
                command.Parameters.AddWithValue("@message", log.Message);
                command.Parameters.AddWithValue("@logLevel", log.LogLevel);
                command.Parameters.AddWithValue("@category", log.Category);
                command.Parameters.AddWithValue("@username", log.Username);
                command.Parameters.AddWithValue("@operation", log.Operation.ToString());
                command.Parameters.AddWithValue("@success", log.Success);

                // Execution of SQL
                int rows = await command.ExecuteNonQueryAsync();

                connection.Close();

                if (rows == 1)
                {
                    result.IsSuccessful = true;
                    result.Message = string.Empty;

                } else
                {
                    result.IsSuccessful = false;
                    result.Message = $"Rows affected were not 1. It was {rows}";
                }

                return result;
            }
        }

        public List<Log> GetLogData(UserAccount userAccount, Operations userOperation)
        {
            List<Log> logs = new List<Log>();
            var connectionString = new MySqlConnectionStringBuilder
            {
                Server = "localhost",
                Port = 3306,
                UserID = "HAGSJP.WeCasa.SqlUser",
                Password = "cecs491",
                Database = "HAGSJP.WeCasa"
            }.ConnectionString;

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var result = new Result();

                // Select SQL statement
                var selectSql = @"SELECT * FROM `Logs` 
                                    WHERE `Username` = @username 
                                    AND `Operation` = @operation 
                                    AND `Timestamp` >= NOW() - INTERVAL 1 DAY
                                    ORDER BY `Timestamp` ASC;";

                var command = connection.CreateCommand();
                command.CommandText = selectSql;
                command.Parameters.AddWithValue("@username".ToLower(), userAccount.Username.ToLower());
                command.Parameters.AddWithValue("@operation", userOperation.ToString());

                // Execution of SQL
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var message = reader.GetString(1);
                    LogLevels logLevel = (LogLevels)Enum.Parse(typeof(LogLevels), reader.GetString(2));
                    var category = reader.GetString(3);
                    var timestamp = reader.GetDateTime(4);
                    var username = reader.GetString(5);
                    Operations op = (Operations)Enum.Parse(typeof(Operations), reader.GetString(6));
                    UserOperation operation = new UserOperation(op, 1);
                    logs.Add(new Log(message, logLevel, category, timestamp, username, operation));
                }
                return logs;
            }
        }
    }
}
