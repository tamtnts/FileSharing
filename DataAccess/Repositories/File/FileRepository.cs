using DataAccess.Repositories.Generic;

namespace DataAccess.Repositories.File
{
    public class FileRepository : GenericRepository<Models.File>, IFileRepository
    {
        public FileRepository(FileSharingContext context) : base(context)
        {
        }
    }
}
