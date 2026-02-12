using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entities;

namespace DataAccessLayer.Data
{
    public class CustomerDBContext : DbContext
    {
        public CustomerDBContext(DbContextOptions options) : base(options) { }

        public DbSet<CustomerModel> Customer { get; set; }

    }
   
    }
    
        


