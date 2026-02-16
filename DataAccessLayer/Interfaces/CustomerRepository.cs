using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerDBContext customerDBContext;
       
        public CustomerRepository(CustomerDBContext customerDBContext)
        {
            this.customerDBContext = customerDBContext;
        }

        public async Task<IEnumerable<CustomerModel>> GetCustomerAsync()
        {
            return await customerDBContext.Customer.ToListAsync();
        }

    }
}
