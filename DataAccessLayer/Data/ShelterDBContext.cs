using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Data
{
    public class ShelterDBContext : DbContext
    {
        public ShelterDBContext(DbContextOptions<ShelterDBContext> options)
        : base(options) { }

        public DbSet<ShelterModel> Shelter { get; set; }
    }
}
