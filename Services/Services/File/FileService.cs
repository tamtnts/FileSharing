using Amazon.S3;
using Amazon.S3.Model;
using DataAccess.Repositories.File;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Amazon.Runtime;

namespace Services.Services.File
{
    public class FileService : IFileService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;
        private readonly IFileRepository _fileRepo;

        public FileService(IConfiguration configuration, IFileRepository fileRepo)
        {
            var accessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID") ?? configuration["AWS:AccessKey"];
            var secretKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY") ?? configuration["AWS:SecretKey"];
            var region = Amazon.RegionEndpoint.GetBySystemName(configuration["AWS:Region"]);

            var awsCredentials = new BasicAWSCredentials(accessKey, secretKey);
            _s3Client = new AmazonS3Client(awsCredentials, region);
            _bucketName = configuration["AWS:BucketName"];
            _fileRepo = fileRepo;
        }

        public async Task<string> UploadFile(IFormFile file, int userId, bool autoDelete)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is null or empty");

            var fileName = $"{file.FileName}";
            using (var stream = file.OpenReadStream())
            {
                var putRequest = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = fileName,
                    InputStream = stream,
                    ContentType = file.ContentType
                };

                var response = await _s3Client.PutObjectAsync(putRequest);
            }

            var fileRecord = new DataAccess.Models.File
            {
                UserId = userId,
                DownloadCount = 0,
                FileName = fileName,
                FileUrl = $"https://{_bucketName}.s3.amazonaws.com/{fileName}",
                AutoDelete = autoDelete,
            };

            await _fileRepo.AddAsync(fileRecord);
            await _fileRepo.SaveChangesAsync();

            return $"https://{_bucketName}.s3.amazonaws.com/{fileName}";
        }

        public async Task<DataAccess.Models.File> GetFileById(Guid id)
        {
            var file = await _fileRepo.GetByIdAsync(id);
            if (file == null)
                throw new FileNotFoundException("File not found");

            file.DownloadCount++;
            if (file.AutoDelete && file.DownloadCount >= 1)
            {
                await _fileRepo.DeleteAsync(file);
            }
            else
            {
                await _fileRepo.UpdateAsync(file);
            }

            await _fileRepo.SaveChangesAsync();
            return file;
        }

        public async Task<Stream> DownloadFile(string fileName)
        {
            var request = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = fileName
            };

            var response = await _s3Client.GetObjectAsync(request);
            return response.ResponseStream;
        }
    }
}
