using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;


namespace DataAccessLayer.Data
{
    public class DogParksDBContext : DbContext
    {
        public DogParksDBContext(DbContextOptions<DogParksDBContext> options) : base(options) { }
        public DbSet<DogParksModel> DogParks { get; set; }
    }
}
