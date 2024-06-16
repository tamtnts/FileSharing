using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
