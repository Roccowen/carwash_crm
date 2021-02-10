using carwash.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Xamarin.Essentials;

namespace carwash
{
    class
        DBContext : DbContext
    {
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Service> Services { get; set; } 
        public DBContext()
        {
            SQLitePCL.Batteries_V2.Init();
            this.Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "database.db3");

            optionsBuilder
                .UseSqlite($"Filename={dbPath}");
        }
    }
}
