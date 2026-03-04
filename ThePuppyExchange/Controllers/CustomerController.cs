using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;


namespace ThePuppyExchange.Controllers
{
    public class CustomerController : Controller
    {
        private readonly CustomerDBContext customerDBContext;
        private readonly PrivilegeDBContext privilegeDBContext;
        private readonly PuppyDbContext puppyDbContext;


        public CustomerController(CustomerDBContext customerDBContext,
        PrivilegeDBContext privilegeDBContext,
        PuppyDbContext puppyDbContext)
        {
            this.customerDBContext = customerDBContext;
            this.privilegeDBContext = privilegeDBContext;
            this.puppyDbContext = puppyDbContext;
        }
        public IActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Registration(CustomerModel customerSignUp)
        {
            ViewData["ShowLogout"] = true;

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
            var userPriveleges = await privilegeDBContext.AccountPrivileges.ToListAsync();

            foreach (CustomerModel customer in customers)
            {
                if (customer.email == customerLogin.email && customer.password == customerLogin.password)
                {
                    customer.cookie = true;
                    await customerDBContext.SaveChangesAsync();

                    // Save to session
                    HttpContext.Session.SetInt32("CustomerId", customer.id);

                    UserPrivilegeModel user = await privilegeDBContext.AccountPrivileges.FirstOrDefaultAsync(x => x.customer_Id == customer.id);
                    if (user == null)
                    {
                        return RedirectToAction("Catalog", "Puppy");
                    }

                    if (user.privilege == "admin")
                    {
                        HttpContext.Session.SetString("IsAdmin", "true");
                        return RedirectToAction("AdminPanel", "Admin");
                    }
                    return RedirectToAction("Catalog", "Puppy");
                }
            }
            return RedirectToAction("Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            int? idCache = HttpContext.Session.GetInt32("CustomerId");
            var customer = await customerDBContext.Customer.FirstOrDefaultAsync(x => x.id == idCache);
            customer.cookie = false;
            await customerDBContext.SaveChangesAsync();

            HttpContext.Session.Clear();
            return RedirectToAction("Home");
        }

        public IActionResult Home()
        {
            return View();
        }

        public async Task<IActionResult> Cart()
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerId");

            if (customerId == null)
            {
                return RedirectToAction("Login", "Customer");
            }

            var cartItems = await (
                from c in puppyDbContext.Cart
                join p in puppyDbContext.Puppy on c.product_id equals p.product_id
                where c.customer_id == customerId
                select new CartModel
                {
                    id = c.id,
                    customer_id = c.customer_id,
                    product_id = c.product_id,
                    quantity = c.quantity,
                    name = p.name,
                    breed = p.breed,
                    fee = p.fee,
                    profile_pic = p.profile_pic
                }
            ).ToListAsync();

            return View(cartItems);
        }

        public IActionResult AddToCart(int puppyId)
        {
            var cartItem = new CartModel
            {
                customer_id = HttpContext.Session.GetInt32("CustomerId") ?? 0,
                product_id = puppyId,
                quantity = 1
            };

            puppyDbContext.Cart.Add(cartItem);
            puppyDbContext.SaveChanges();

            return RedirectToAction("Catalog", "Puppy");
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int cartId)
        {
            var cartItem = puppyDbContext.Cart.FirstOrDefault(c => c.id == cartId);
            if (cartItem != null)
            {
                puppyDbContext.Cart.Remove(cartItem);
                puppyDbContext.SaveChanges();
            }

            return RedirectToAction("Cart");
        }
    }
}
