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
            var puppies = await puppyDbContext.Puppy.ToListAsync();
            return View(puppies);
        }

    }
}
