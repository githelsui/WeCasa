using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Azure;
using HAGSJP.WeCasa.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Client;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace HAGSJP.WeCasa.sqlDataAccess
{
    public class FilesS3DAO
    {
        // Configuring AWS S3 client for hagsjp.wecasa.s3 user
        private AmazonS3Client _client = new AmazonS3Client(
            "AKIA2K6ZUAG72RZQSEVE", // access key
            "IvAMJp3Wx6qTzNxYDYCcUJGI7UtecLxJCO6APkDO", // secret access key
            Amazon.RegionEndpoint.USEast2
        );
        private string _bucketName = "wecasa-group-files";

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

        public async Task<Result> CreateBucket(string bucketName)
        {
            var result = new Result();
             try
            {
                var response = await _client.PutBucketAsync(bucketName);
                result.Message = response.ResponseMetadata.ToString();
                result.ErrorStatus = response.HttpStatusCode;
            } 
            catch(AmazonS3Exception ex)
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
            var request = new ListObjectsV2Request{ BucketName = _bucketName }; //BucketName = _bucketName+groupId,
            ListObjectsV2Response response;

            do
            {
                response = await _client.ListObjectsV2Async(request);
                foreach (var s3Obj in response.S3Objects)
                {
                    var getObjectRequest = new GetObjectRequest
                    {
                        //BucketName = _bucketName+groupId, 
                        BucketName = _bucketName,
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

        public async Task<S3Result> UploadFile(IFormFile file, string bucketName, string? prefix)
        {
            var result = new S3Result();
            var request = new PutObjectRequest()
            {
                BucketName = bucketName,
                Key = string.IsNullOrEmpty(prefix) ? file.FileName : $"{prefix?.TrimEnd('/')}/{file.FileName}",
                InputStream = file.OpenReadStream()
            };
            request.Metadata.Add("Content-Type", file.ContentType);
            await _client.PutObjectAsync(request);
            return result;
        }

    }
}
