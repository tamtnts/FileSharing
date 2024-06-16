using DataAccess.Models;
using DataAccess.Repositories.Generic;
using DataAccess.Repositories.Text;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.Text
{
    public class TextService : ITextService
    {
        private readonly ITextRepository _textRepo;
        private IGenericRepository<DataAccess.Models.Text> _genericRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TextService(ITextRepository textRepo, IGenericRepository<DataAccess.Models.Text> genericRepo, IHttpContextAccessor httpContextAccessor)
        {
            _textRepo = textRepo;
            _genericRepo = genericRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> UploadText(string textContent, bool autoDelete, int userId)
        {
            var textRecord = new DataAccess.Models.Text
            {
                UserId = userId,
                TextContent = textContent,
                AutoDelete = autoDelete,
            };

            await _textRepo.AddTextAsync(textRecord);

            var absoluteUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/text/{textRecord.Id}";
            return absoluteUrl;
        }

        public async Task<DataAccess.Models.Text> GetTextById(Guid id)
        {
            var text = await _textRepo.GetTextByIdAsync(id);
            if (text == null)
                throw new FileNotFoundException("Text not found");

            text.AccessCount++;
            if (text.AutoDelete && text.AccessCount >= 1)
                await _textRepo.RemoveTextAsync(text);
            else {
                await _genericRepo.UpdateAsync(text);
                await _genericRepo.SaveChangesAsync();
            }
                
            return text;
        }
    }
}
