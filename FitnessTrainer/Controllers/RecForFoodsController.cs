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
using FitnessTrainer.Services.Interfaces;
using X.PagedList;

namespace FitnessTrainer.Controllers
{
    [Authorize(Policy = "Administrator")]
    public class RecForFoodsController : Controller
    {
        private ApplicationDbContext _context;
        private IRecForFoodService _recForFoodService;

        public RecForFoodsController(ApplicationDbContext context, IRecForFoodService recForFoodService)
        {
            _context = context;
            _recForFoodService = recForFoodService;
        }

        // GET: RecForFoods
        public async Task<IActionResult> Index(string searchString, int? page, int pageSize = 5)
        {
            ViewData["CurrentFilter"] = searchString;

            List<RecForFoodViewModel> recForFoodList = await _recForFoodService.GetRecForFoods(searchString);

            int pageNumber = (page ?? 1);
            ViewBag.recsList = await recForFoodList.ToPagedListAsync(pageNumber, pageSize);
            ViewBag.NameOfActivePage = "RecForFoodsIndex";
            return View(recForFoodList);
        }

        // GET: RecForFoods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _recForFoodService.GetRecForFoodById(id);

            if (model == null)
            {
                return NotFound();
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
                await _recForFoodService.CreateRecForFood(recForFood);
                return RedirectToAction(nameof(Index));
            }
            RecForFoodViewModel model = await _recForFoodService.RecForFoodIntoViewModel(recForFood);
            return View(model);
        }

        // GET: RecForFoods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RecForFoodViewModel model = await _recForFoodService.GetRecForFoodById(id);

            if (model == null)
            {
                return NotFound();
            }

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
                    await _recForFoodService.UpdateRecForFood(recForFood);
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
            RecForFoodViewModel model = await _recForFoodService.GetRecForFoodById(recForFood.Id);
            return View(model);
        }

        // GET: RecForFoods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RecForFoodViewModel model = await _recForFoodService.GetRecForFoodById(id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: RecForFoods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _recForFoodService.DeleteRecForFood(id);
            return RedirectToAction(nameof(Index));
        }

        private bool RecForFoodExists(int id)
        {
            return _context.RecForFoods.Any(e => e.Id == id);
        }

        [HttpPost]
        public async Task<IActionResult> AddFoodToPlan(int? planid, int? foodid)
        {
            if(planid == null || foodid == null)
            {
                BadRequest();
            }

            await _recForFoodService.AddFoodToRecForFood(planid, foodid);
            string url = "Edit/" + planid;

            return Redirect(url);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFoodFromPlan(int? idplan, int? idfood)
        {
            int foodid = idfood ?? 0;
            if(foodid == 0)
            {
                return NotFound();
            }

            await _recForFoodService.DeleteFoodFromRecForFood(idplan, idfood);

            string url = "Edit/" + idplan;

            return Redirect(url);
        }
    }
}
