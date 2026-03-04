using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Data
{
    public class PuppyDbContext : DbContext
    {
        public PuppyDbContext(DbContextOptions<PuppyDbContext> options)
        : base(options) { }

        public DbSet<PuppyModel> Puppy { get; set; }
        public DbSet<CartModel> Cart { get; set; }


    }
}
