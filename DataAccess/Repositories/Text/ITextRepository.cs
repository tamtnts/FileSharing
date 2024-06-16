using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Text
{
    public interface ITextRepository 
    {
        Task<Models.Text> GetTextByIdAsync(Guid id);
        Task AddTextAsync(Models.Text textRecord);
        Task RemoveTextAsync(Models.Text textRecord);
    }
}
