using BusinessLogicLayer;
using DataAccessLayer.Data;
using Microsoft.AspNetCore.Cors.Infrastructure;
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

        public async Task<IActionResult> Catalog(string selectedBreed, string selectedSex)
        {
            var query = puppyDbContext.Puppy.AsQueryable();

            if (!string.IsNullOrEmpty(selectedBreed))
            {
                query = query.Where(p => p.breed == selectedBreed);
            }

            if (!string.IsNullOrEmpty(selectedSex))
            {
                query = query.Where(p => p.sex.ToLower() == selectedSex.ToLower());
            }

            var puppies = await query.ToListAsync();

            var allBreeds = await puppyDbContext.Puppy.Select(p => p.breed).Distinct().ToListAsync();

            var allSexes = new List<string> { "M", "F" };

            ViewData["Breeds"] = allBreeds;
            ViewData["Sexes"] = allSexes;
            ViewData["SelectedBreed"] = selectedBreed;
            ViewData["SelectedSex"] = selectedSex;
            ViewData["ShowLogout"] = true;

            return View(puppies);
        }

        public IActionResult ProductPage(int id)
        {
            var puppy = puppyDbContext.Puppy
                .FirstOrDefault(p => p.product_id == id);

            if (puppy == null)
                return NotFound();

            return View(puppy);
        }
    }
}
