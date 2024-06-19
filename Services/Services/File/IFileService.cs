using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.File
{
    public interface IFileService
    {
        Task<string> UploadFile(IFormFile file, int userId, bool autoDelete);
        Task<DataAccess.Models.File> GetFileById(Guid id);
        Task<Stream> DownloadFile(string fileName);
    }
}
