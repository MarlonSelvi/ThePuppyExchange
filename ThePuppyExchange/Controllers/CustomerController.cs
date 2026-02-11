using BusinessLogicLayer;
using Microsoft.AspNetCore.Mvc;

namespace ThePuppyExchange.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService customerService;

        public CustomerController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }

        public async Task<IActionResult> Index()
        {
            var customer = await customerService.GetCustomerAsync();
            return View(customer);
        }
    }
}
