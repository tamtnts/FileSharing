using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class FileSharingContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Models.File> Files { get; set; }
        public DbSet<Text> Texts { get; set; }

        public FileSharingContext(DbContextOptions<FileSharingContext> options) : base(options) { }
    }
}
