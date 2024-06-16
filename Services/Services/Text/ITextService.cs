using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.Text
{
    public interface ITextService
    {
        /*Task<DataAccess.Models.Text> GetTextByIdAsync(int id);*/
        Task<string> UploadText(string textContent, bool autoDelete, int userId);
        Task<DataAccess.Models.Text> GetTextById(Guid id);
    }
}
