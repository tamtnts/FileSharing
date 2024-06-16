using DataAccess.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Text
{ 
    public class TextRepository : GenericRepository<Models.Text>, ITextRepository
    {
        public TextRepository(FileSharingContext context) : base(context) { }

        public async Task<Models.Text> GetTextByIdAsync(Guid id)
        {
            return await _context.Texts.SingleOrDefaultAsync(t => t.Id == id);
        }

        public async Task AddTextAsync(Models.Text text)
        {
            await _context.Texts.AddAsync(text);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveTextAsync(Models.Text text)
        {
            _context.Texts.Remove(text);
            await _context.SaveChangesAsync();
        }
    }
}
