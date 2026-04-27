using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using ThePuppyExchange.Models;


namespace ThePuppyExchange.Controllers
{
    public class CustomerController : Controller
    {
        private readonly CustomerDBContext customerDBContext;
        private readonly PrivilegeDBContext privilegeDBContext;
        private readonly PuppyDbContext puppyDbContext;
        private readonly DogParksDBContext dogParksDBContext;


        public CustomerController(CustomerDBContext customerDBContext,
        PrivilegeDBContext privilegeDBContext,
        PuppyDbContext puppyDbContext,
        DogParksDBContext dogParksDBContext)
        {
            this.customerDBContext = customerDBContext;
            this.privilegeDBContext = privilegeDBContext;
            this.puppyDbContext = puppyDbContext;
            this.dogParksDBContext = dogParksDBContext;
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
                    HttpContext.Session.SetString("CustomerName", customer.fname);

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
                 profile_pic = p.profile_pic,
                 maxQuantity = p.quantity
             }
         ).ToListAsync();

            return View(cartItems);
        }

        public IActionResult Map()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetDogParks()
        {
            var parks = dogParksDBContext.DogParks
                .Select(p => new
                {
                    id = p.id,
                    name = p.name,
                    latitude = p.lat,
                    longitude = p.lng
                })
                .ToList();

            return Ok(parks);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCartQuantity(int cartId, int newQuantity)
        {
            var cartItem = await puppyDbContext.Cart.FirstOrDefaultAsync(c => c.id == cartId);
            if (cartItem == null)
            {
                return RedirectToAction("Cart");
            }

            // Get the max quantity from the puppy table
            var puppy = await puppyDbContext.Puppy.FirstOrDefaultAsync(p => p.product_id == cartItem.product_id);
            if (puppy == null)
            {
                return RedirectToAction("Cart");
            }

            //Between 1 and the puppy's available quantity
            if (newQuantity < 1) newQuantity = 1;
            if (newQuantity > puppy.quantity) newQuantity = puppy.quantity;

            cartItem.quantity = newQuantity;
            puppyDbContext.Cart.Update(cartItem);
            await puppyDbContext.SaveChangesAsync();

            return RedirectToAction("Cart");
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

        public IActionResult Checkout()
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerId");

            var cartItems = (from cart in puppyDbContext.Cart
                             join puppy in puppyDbContext.Puppy
                             on cart.product_id equals puppy.product_id
                             where cart.customer_id == customerId
                             select new Checkout
                             {
                                 id = cart.id,
                                 product_id = cart.product_id,
                                 quantity = cart.quantity,

                                 name = puppy.name,
                                 breed = puppy.breed,
                                 fee = puppy.fee,
                                 profile_pic = puppy.profile_pic
                             }).ToList();

            if (!cartItems.Any())
            {
                return RedirectToAction("Cart");
            }

            return View(cartItems);
        }


        [HttpPost]
        public async Task<IActionResult> PlaceOrder()
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerId");

            if (customerId == null)
            {
                return RedirectToAction("Login", "Customer");
            }

            // Get cart items
            var cartItems = await puppyDbContext.Cart
                .Where(c => c.customer_id == customerId)
                .ToListAsync();

            if (cartItems == null || cartItems.Count == 0)
            {
                return RedirectToAction("Cart");
            }

            // Create order
            var order = new OrderModel
            {
                customer_id = customerId.Value,
                date = DateTime.UtcNow
            };

            puppyDbContext.Order.Add(order);
            await puppyDbContext.SaveChangesAsync();

            // Create order items & reduce puppy quantity
            foreach (var cartItem in cartItems)
            {
                var orderItem = new OrderItemModel
                {
                    order_id = order.id,
                    product_id = cartItem.product_id,
                    quantity = cartItem.quantity
                };

                puppyDbContext.OrderItems.Add(orderItem);

                var puppy = await puppyDbContext.Puppy.FirstOrDefaultAsync(p => p.product_id == cartItem.product_id);
                if (puppy != null)
                {
                    puppy.quantity -= cartItem.quantity;
                    if (puppy.quantity < 0) puppy.quantity = 0;
                }
            }

            // Clear the cart
            puppyDbContext.Cart.RemoveRange(cartItems);

            await puppyDbContext.SaveChangesAsync();

            return RedirectToAction("OrderConfirmation", new { orderId = order.id });
        }

        public async Task<IActionResult> OrderConfirmation(int orderId)
        {
            var order = await puppyDbContext.Order.FirstOrDefaultAsync(o => o.id == orderId);

            if (order == null)
                return NotFound();

            return View(order);
        }

        public async Task<IActionResult> OrderHistory()
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerId");

            if (customerId == null)
            {
                return RedirectToAction("Login", "Customer");
            }

            // Get previous orders
            var orders = await puppyDbContext.Order
                .Where(o => o.customer_id == customerId)
                .OrderByDescending(o => o.date)
                .ToListAsync();

            // Get all puppy info
            var orderItems = await (
                from oi in puppyDbContext.OrderItems
                join p in puppyDbContext.Puppy on oi.product_id equals p.product_id
                join o in puppyDbContext.Order on oi.order_id equals o.id
                where o.customer_id == customerId
                select new
                {
                    oi.order_id,
                    oi.quantity,
                    p.name,
                    p.breed,
                    p.fee,
                    p.profile_pic
                }
            ).ToListAsync();

            // Group orders
            var orderHistory = orders.Select(o => new OrderHistoryViewModel
            {
                order_id = o.id,
                date = o.date,
                items = orderItems
                    .Where(oi => oi.order_id == o.id)
                    .Select(oi => new OrderHistoryItemViewModel
                    {
                        name = oi.name,
                        breed = oi.breed,
                        fee = oi.fee,
                        quantity = oi.quantity,
                        profile_pic = oi.profile_pic
                    }).ToList()
            }).ToList();

            return View(orderHistory);
        }
    }
}
