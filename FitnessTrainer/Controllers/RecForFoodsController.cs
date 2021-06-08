using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FitnessTrainer.DataAccess.DbContexts;
using FitnessTrainer.DomainEntities.Entity;
using Microsoft.AspNetCore.Authorization;
using FitnessTrainer.ViewModels;
using X.PagedList;

namespace FitnessTrainer.Controllers
{
    [Authorize(Policy = "Administrator")]
    public class RecForFoodsController : Controller
    {
        private ApplicationDbContext _context;

        public RecForFoodsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RecForFoods
        public async Task<IActionResult> Index(string searchString, int? page, int pageSize = 5)
        {
            ViewData["CurrentFilter"] = searchString;

            var recForFoods = from m in _context.RecForFoods select m;

            if(!String.IsNullOrEmpty(searchString))
            {
                recForFoods = recForFoods.Where(m => m.Name.Contains(searchString));
            }

            int pageNumber = (page ?? 1);
            ViewBag.recsList = await recForFoods.ToPagedListAsync(pageNumber, pageSize);

            return View(recForFoods);
        }

        // GET: RecForFoods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recForFood = await _context.RecForFoods.Include(c => c.Foods)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (recForFood == null)
            {
                return NotFound();
            }

            RecForFoodViewModel model = new RecForFoodViewModel()
            {
                Id = recForFood.Id,
                Name = recForFood.Name,
                Description = recForFood.Description
            };

            if(recForFood.Foods == null)
            {
                ViewBag.foodList = null;
            } else
            {
                ViewBag.foodList = recForFood.Foods;
            }

            return View(model);
        }

        // GET: RecForFoods/CreatePlan
        [HttpGet]
        public IActionResult CreatePlan()
        {
            return View();
        }

        // POST: RecForFoods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePlan([Bind("Id,Name,Description")] RecForFood recForFood)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recForFood);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(recForFood);
        }

        // GET: RecForFoods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RecForFood recForFood = _context.RecForFoods.Include(a => a.Foods).FirstOrDefault(x => x.Id == id);

            if (recForFood == null)
            {
                return NotFound();
            }
            
            List<FoodViewModel> vv = new List<FoodViewModel>();

            if (recForFood.Foods != null)
            {
                

                foreach (var s in recForFood.Foods)
                {
                    FoodViewModel food = new FoodViewModel()
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Calories = s.Calories,
                        Fats = s.Fats,
                        Carbohydrates = s.Carbohydrates,
                        Proteins = s.Proteins
                    };
                    vv.Add(food);
                }
            }

            RecForFoodViewModel model = new RecForFoodViewModel()
            {
                Id = recForFood.Id,
                Name = recForFood.Name,
                Description = recForFood.Description,
                Foods = vv
            };

            List<Food> foods = await _context.Foods.ToListAsync();
            ViewBag.foodList = foods;

            return View(model);
        }

        // POST: RecForFoods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] RecForFood recForFood)
        {
            if (id != recForFood.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recForFood);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecForFoodExists(recForFood.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(recForFood);
        }

        // GET: RecForFoods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recForFood = await _context.RecForFoods
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recForFood == null)
            {
                return NotFound();
            }

            return View(recForFood);
        }

        // POST: RecForFoods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recForFood = await _context.RecForFoods.FindAsync(id);
            _context.RecForFoods.Remove(recForFood);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecForFoodExists(int id)
        {
            return _context.RecForFoods.Any(e => e.Id == id);
        }

        [HttpPost]
        public IActionResult AddFoodToPlan(int? planid, int? foodid)
        {
            if(planid == null || foodid == null)
            {
                BadRequest();
            }

            RecForFood plan = _context.RecForFoods.FindAsync(planid).GetAwaiter().GetResult();

            Food food = _context.Foods.FindAsync(foodid).GetAwaiter().GetResult();

            plan.Foods.Add(food);
            _context.SaveChanges();
            string url = "Edit/" + planid;

            return Redirect(url);
        }

        [HttpPost]
        public IActionResult DeleteFoodFromPlan(int? idplan, int? idfood)
        {
            int foodid = idfood ?? 0;
            if(foodid == 0)
            {
                return NotFound();
            }
            RecForFood rec = _context.RecForFoods.Include(c => c.Foods).FirstOrDefault(i => i.Id == idplan);
            Food del = _context.Foods.Find(foodid);
            rec.Foods.Remove(del);
            _context.SaveChanges();

            string url = "Edit/" + idplan;

            return Redirect(url);
        }
    }
}
