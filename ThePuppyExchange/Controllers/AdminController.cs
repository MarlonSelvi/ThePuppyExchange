using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ThePuppyExchange.Controllers
{
    public class AdminController : Controller
    {
        public readonly PuppyDbContext puppyDbContext;


        public AdminController(PuppyDbContext puppyDBContext)
        {
            this.puppyDbContext = puppyDBContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AdminPanel()
        {
            ViewData["ShowLogout"] = true;
            return View();
        }

        public async Task<IActionResult> AlterDB()
        {
            ViewData["ShowLogout"] = true;

            var puppies = await puppyDbContext.Puppy.ToListAsync();

            return View(puppies);
        }

        public IActionResult EditPuppy(int id)
        {
            ViewData["ShowLogout"] = true;

            var puppy = puppyDbContext.Puppy
                .FirstOrDefault(p => p.product_id == id);

            if (puppy == null)
                return NotFound();

            return View(puppy);
        }

        [HttpPost]
        public IActionResult Alterpuppy(PuppyModel puppy)
        {
            ViewData["ShowLogout"] = true;

            var puppyToAlter = puppyDbContext.Puppy
                .FirstOrDefault(p => p.product_id == puppy.product_id);
            if (puppyToAlter == null)
            {
                return NotFound();
            }
            else if (puppyToAlter != null)
            {
                puppyToAlter.name = puppy.name;
                puppyToAlter.breed = puppy.breed;
                puppyToAlter.age = puppy.age;
                puppyToAlter.fee = puppy.fee;
                puppyToAlter.sex = puppy.sex;
                puppyToAlter.description = puppy.description;
                puppyToAlter.profile_pic = puppy.profile_pic;
            }
            puppyDbContext.SaveChanges();

            return RedirectToAction("AlterDB", "Admin");
        }

        public IActionResult AddPuppy()
        {
            ViewData["ShowLogout"] = true;

            return View();
        }

        [HttpPost]
        public IActionResult AddPuppy(PuppyModel puppy)
        {

            puppy.shelter_id = 1;
            puppyDbContext.Puppy.Add(puppy);
            puppyDbContext.SaveChanges();

            return RedirectToAction("AlterDB", "Admin");
        }

        public IActionResult DeletePuppy(int id)
        {
            ViewData["ShowLogout"] = true;

            var puppyToDelete = puppyDbContext.Puppy
                .FirstOrDefault(p => p.product_id == id);
            if (puppyToDelete == null)
            {
                return NotFound();
            }
            else if (puppyToDelete != null)
            {
                puppyDbContext.Puppy.Remove(puppyToDelete);
                puppyDbContext.SaveChanges();
            }

            return RedirectToAction("AlterDB", "Admin");
        }
    }
}
