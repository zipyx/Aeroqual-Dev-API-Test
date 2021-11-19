using ApiLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiLibrary.Data
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options) : base(options) {}

        public DbSet<People> People { get; set; } = null!;
        public DbSet<Person> Person { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity <People>(p =>
            {
                p.HasNoKey();
            });
        }
    }
}
