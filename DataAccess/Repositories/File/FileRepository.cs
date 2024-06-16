using DataAccess.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.File
{
    public class FileRepository : IFileRepository
    {
        /*private readonly GenericRepository<Models.File> _repository;
        public FileRepository(GenericRepository<Models.File> repository)
        {
            _repository = repository;
        }

        public async Task<Models.File> GetFileByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddFileAsync(Models.File file)
        {
            await _repository.AddAsync(file);
        }

        public async Task RemoveFileAsync(Models.File file)
        {
            await _repository.DeleteAsync(file);
        }*/
    }
}
