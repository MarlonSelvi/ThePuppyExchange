using Azure.Identity;
using BusinessLogicLayer;
using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace ThePuppyExchange.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService customerService;
        private readonly CustomerDBContext customerDBContext;
        private static int idCache = 0;


        public CustomerController(CustomerDBContext customerDBContext)
        {
            this.customerDBContext = customerDBContext;
        }
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(CustomerModel customerSignUp)
        {
            var customers = await customerDBContext.Customer.ToListAsync();

            foreach (CustomerModel customer in customers)
            {
                if (customer.email == customerSignUp.email && customer.password == customerSignUp.password)
                {
                    return RedirectToAction("Login");
                }
            }
            var user = new CustomerModel()
            {
                fname = customerSignUp.fname,
                lname = customerSignUp.lname,
                email = customerSignUp.email,
                password = customerSignUp.password,

            };

            await customerDBContext.Customer.AddAsync(user);
            await customerDBContext.SaveChangesAsync();
            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(CustomerModel customerLogin)
        {
            var customers = await customerDBContext.Customer.ToListAsync();

            foreach (CustomerModel customer in customers) {
                if (customer.email == customerLogin.email && customer.password == customerLogin.password)
                {
                    customer.cookie = true;
                    idCache = customer.id;

                    await customerDBContext.SaveChangesAsync();
                    return RedirectToAction("Catalog");
                }
            }
            return RedirectToAction("Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var customer = await customerDBContext.Customer.FirstOrDefaultAsync(x => x.id == idCache);
            customer.cookie = false;
           await customerDBContext.SaveChangesAsync();
           return RedirectToAction("Home");
        }

        public IActionResult Home()
        {
            return View();
        }
        public IActionResult Catalog()
        {
            return View();
        }
    }
}
