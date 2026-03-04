using BusinessLogicLayer;
using DataAccessLayer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ThePuppyExchange.Controllers
{
    public class PuppyController : Controller
    {
        private readonly IPuppyService puppyService;
        private readonly PuppyDbContext puppyDbContext;

        public PuppyController(PuppyDbContext puppyDbContext)
        {
            this.puppyDbContext = puppyDbContext;
        }

        public async Task<IActionResult> Catalog()
        {
            ViewData["ShowLogout"] = true;
            var puppies = await puppyDbContext.Puppy.ToListAsync();

            return View(puppies);
        }

        public IActionResult ProductPage(int id)
        {
            ViewData["ShowLogout"] = true;

            var puppy = puppyDbContext.Puppy
                .FirstOrDefault(p => p.product_id == id);

            if (puppy == null)
                return NotFound();

            return View(puppy);
        }
    }
}
