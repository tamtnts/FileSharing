using Microsoft.AspNetCore.Http;

namespace Services.Services.File
{
    public interface IFileService
    {
        Task<string> UploadFile(IFormFile file, int userId, bool autoDelete);
        Task<DataAccess.Models.File> GetFileById(Guid id);
        Task<Stream> DownloadFile(string fileName);
    }
}
