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

namespace FitnessTrainer.Controllers
{
    [Authorize(Policy = "Administrator")]
    public class ExercisesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ExercisesController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
        }

        // GET: Exercises
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var exercises = from m in _context.Exercises select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                exercises = _context.Exercises.Where(s => s.Name.Contains(searchString));
            }

            return View(await exercises.ToListAsync());
        }

        // GET: Exercises/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exercise = await _context.Exercises
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exercise == null)
            {
                return NotFound();
            }

            return View(exercise);
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

                Exercise exercise = new Exercise()
                {
                    Name = model.Name,
                    ImagePath = uniqueFileName,
                    Description = model.Description,
                    NumberOfApproaches = model.NumberOfApproaches,
                    NumberOfRepetitions = model.NumberOfRepetitions,
                    TimeBetweenSets = model.TimeBetweenSets,
                    RestTimeAtTheEnd = model.RestTimeAtTheEnd
                };

                _context.Add(exercise);
                await _context.SaveChangesAsync();
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

            var exercise = await _context.Exercises.FindAsync(id);
            if (exercise == null)
            {
                return NotFound();
            }
            return View(exercise);
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

                        Exercise exercise = new Exercise()
                        {
                            Id = model.Id,
                            Name = model.Name,
                            ImagePath = uniqueFileName,
                            Description = model.Description,
                            NumberOfApproaches = model.NumberOfApproaches,
                            NumberOfRepetitions = model.NumberOfRepetitions,
                            TimeBetweenSets = model.TimeBetweenSets,
                            RestTimeAtTheEnd = model.RestTimeAtTheEnd
                        };
                        _context.Update(exercise);
                    } else
                    {
                        Exercise exercise = new Exercise()
                        {
                            Id = model.Id,
                            Name = model.Name,
                            ImagePath = OldImagePath,
                            Description = model.Description,
                            NumberOfApproaches = model.NumberOfApproaches,
                            NumberOfRepetitions = model.NumberOfRepetitions,
                            TimeBetweenSets = model.TimeBetweenSets,
                            RestTimeAtTheEnd = model.RestTimeAtTheEnd
                        };
                        _context.Update(exercise);
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

            var exercise = await _context.Exercises
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exercise == null)
            {
                return NotFound();
            }

            return View(exercise);
        }

        // POST: Exercises/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exercise = await _context.Exercises.FindAsync(id);
            _context.Exercises.Remove(exercise);
            await _context.SaveChangesAsync();
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
