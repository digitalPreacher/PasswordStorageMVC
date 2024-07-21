using Microsoft.EntityFrameworkCore;
using PasswordStorage.Models;

namespace PasswordStorage.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { 

        }

        public DbSet<Container> Containers { get; set; }
        public DbSet<ContainerItem> Items { get; set; }

    }
}
