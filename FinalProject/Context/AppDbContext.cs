using FinalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Context
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> option):base(option) 
        { 

        }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("users");
            /*base.OnModelCreating(modelBuilder);*/
        }
    }
}
