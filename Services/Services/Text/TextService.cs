using DataAccess.Models;
using DataAccess.Repositories.Generic;
using DataAccess.Repositories.Text;
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

        public TextService(ITextRepository textRepo, IGenericRepository<DataAccess.Models.Text> genericRepo)
        {
            _textRepo = textRepo;
            _genericRepo = genericRepo;
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

            // Return the URL 
            return $"/api/text/{textRecord.Id}";
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
