using Amazon.S3;
using Amazon.S3.Model;
using Azure;
using HAGSJP.WeCasa.Models;
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


        public async Task<S3Result> GetGroupFiles(string groupId)
        {
            var result = new S3Result();

            var request = new ListObjectsV2Request
            {
                BucketName = _bucketName,
            };
            var response = await _client.ListObjectsV2Async(request);

            List<FileModel> files = new List<FileModel>();

            // Getting file information
            foreach (var s3Object in response.S3Objects)
            {
                files.Add(new FileModel(s3Object.Key));
            }
            result.Files = files;
            result.ErrorStatus = response.HttpStatusCode;
            result.Message = $"Number of group files retrieved: {response.KeyCount}";
            result.IsSuccessful = true;
            return result;
        }

    }
}
