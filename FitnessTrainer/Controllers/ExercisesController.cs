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
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using FitnessTrainer.ViewModels;
using X.PagedList;
using FitnessTrainer.Services.Interfaces;

namespace FitnessTrainer.Controllers
{
    [Authorize(Policy = "Administrator")]
    public class ExercisesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IExerciseService _exerciseService;

        public ExercisesController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment, IExerciseService exerciseService)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
            _exerciseService = exerciseService;
        }

        // GET: Exercises
        public async Task<IActionResult> Index(int? page, string searchString, int pageSize = 5)
        {
            ViewData["CurrentFilter"] = searchString;

            List<ExerciseViewModel> model = await _exerciseService.GetExercises(searchString);
            List<Exercise> list = await _exerciseService.GetExercisesInDomainFormat(searchString);
            int pageNumber = (page ?? 1);
            ViewBag.exList = await list.ToPagedListAsync(pageNumber, pageSize);

            return View(model);
        }

        // GET: Exercises/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ExerciseViewModel model = await _exerciseService.GetExerciseById(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // GET: Exercises/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Exercises/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ImagePath,Description,NumberOfApproaches,NumberOfRepetitions,TimeBetweenSets,RestTimeAtTheEnd")] ExerciseViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(model);

                await _exerciseService.CreateExercise(model, uniqueFileName);
                return Redirect("/Exercises/Index");
            }
            return View(model);
        }

        // GET: Exercises/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ExerciseViewModel model = await _exerciseService.GetExerciseById(id);


            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: Exercises/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string OldImagePath, [Bind("Id,Name,ImagePath,Description,NumberOfApproaches,NumberOfRepetitions,TimeBetweenSets,RestTimeAtTheEnd")] ExerciseViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            string uniqueFileName;

            if (ModelState.IsValid)
            {
                try
                {
                    if (model.ImagePath != null)
                    {
                        uniqueFileName = UploadedFile(model);

                        await _exerciseService.UpdateExercise(model, uniqueFileName);
                    } else
                    {
                        await _exerciseService.UpdateExerciseWithOldImage(model, OldImagePath);
                    }
                    
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExerciseExists(model.Id))
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
            return View(model);
        }

        // GET: Exercises/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ExerciseViewModel model = await _exerciseService.GetExerciseById(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: Exercises/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _exerciseService.DeleteExercise(id);
            return RedirectToAction(nameof(Index));
        }

        private bool ExerciseExists(int id)
        {
            return _context.Exercises.Any(e => e.Id == id);
        }

        private string UploadedFile(ExerciseViewModel model)
        {
            string uniqueFileName = null;

            if (model.ImagePath != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImagePath.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ImagePath.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
