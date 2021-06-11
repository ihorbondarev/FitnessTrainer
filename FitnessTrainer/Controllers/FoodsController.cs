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
using FitnessTrainer.Services.Interfaces;
using X.PagedList;
using FitnessTrainer.ViewModels;

namespace FitnessTrainer.Controllers
{
    [Authorize(Policy = "Administrator")]
    public class FoodsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFoodService _foodService;

        public FoodsController(ApplicationDbContext context, IFoodService foodService)
        {
            _context = context;
            _foodService = foodService;
        }

        // GET: Foods
        public async Task<IActionResult> Index(string searchString, int? page, int pageSize = 5)
        {
            ViewData["CurrentFilter"] = searchString;
            List<Food> foods = await _foodService.GetFood(searchString);

            int pageNumber = (page ?? 1);
            ViewBag.foodList = foods.ToPagedList(pageNumber, pageSize);

            return View(foods);
        }

        // GET: Foods/Details/5
        public async Task<IActionResult> Details(int id)
        {
            FoodViewModel food = await _foodService.GetFoodById(id);

            if (food == null)
            {
                return NotFound();
            }

            return View(food);
        }

        // GET: Foods/Create
        public async Task<IActionResult> Create()
        {
            FoodViewModel model = new FoodViewModel();
            return View(model);
        }

        // POST: Foods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Calories,Proteins,Fats,Carbohydrates")] Food food)
        {
            if (ModelState.IsValid)
            {
                await _foodService.CreateFood(food);
                return RedirectToAction(nameof(Index));
            }

            FoodViewModel model = new FoodViewModel()
            {
                Id = food.Id,
                Name = food.Name,
                Calories = food.Calories,
                Carbohydrates = food.Carbohydrates,
                Fats = food.Fats,
                Proteins = food.Proteins
            };

            return View(model);
        }

        // GET: Foods/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            FoodViewModel food = await _foodService.GetFoodById(id);
            if (food == null)
            {
                return NotFound();
            }
            return View(food);
        }

        // POST: Foods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Calories,Proteins,Fats,Carbohydrates")] Food food)
        {
            if (id != food.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _foodService.UpdateFood(food);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FoodExists(food.Id))
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
            var model = await _foodService.FoodIntoViewModel(food);
            return View(model);
        }

        // GET: Foods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            FoodViewModel food = await _foodService.GetFoodById(id);
            
            if (food == null)
            {
                return NotFound();
            }

            return View(food);
        }

        // POST: Foods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _foodService.DeleteFood(id);
            return RedirectToAction(nameof(Index));
        }

        private bool FoodExists(int id)
        {
            return _context.Foods.Any(e => e.Id == id);
        }
    }
}
