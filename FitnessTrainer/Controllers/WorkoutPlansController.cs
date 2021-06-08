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
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace FitnessTrainer.Controllers
{
    [Authorize(Policy = "Administrator")]
    public class WorkoutPlansController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public WorkoutPlansController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
        }

        // GET: WorkoutPlans
        public IActionResult Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            List<WorkoutPlan> plans;
            if (string.IsNullOrEmpty(searchString))
            {
                plans = _context.WorkoutPlans.ToList();
            } else
            {
                plans = _context.WorkoutPlans.Where(s => s.Name.Contains(searchString)).ToList();
            }

            return View(plans);
        }

        // GET: WorkoutPlans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            WorkoutPlan workoutPlan = await _context.WorkoutPlans.Include(c => c.Exercises).Include(s => s.RecForFood)
                .FirstOrDefaultAsync(m => m.Id == id);

            WorkoutPlanViewModel model = new WorkoutPlanViewModel()
            {
                Id = workoutPlan.Id,
                Name = workoutPlan.Name,
                Description = workoutPlan.Description,
                Status = workoutPlan.Status,
                RecForFood = workoutPlan.RecForFood
            };


            if (workoutPlan == null)
            {
                return NotFound();
            }

            List<Exercise> exlist = workoutPlan.Exercises;

            ViewBag.Exercises = exlist;
            ViewBag.ImageString = workoutPlan.ImagePath;

            return View(model);
        }

        // GET: WorkoutPlans/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WorkoutPlans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,ImagePath,Status")] WorkoutPlanViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(model);

                WorkoutPlan plan = new WorkoutPlan()
                {
                    Name = model.Name,
                    Description = model.Description,
                    ImagePath = uniqueFileName,
                    Status = model.Status
                };

                _context.Add(plan);
                await _context.SaveChangesAsync();
                return Redirect("/WorkoutPlans/Index");
            }
            return View(model);
        }

        // GET: WorkoutPlans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            WorkoutPlan plan = _context.WorkoutPlans.Include(a => a.Exercises).Include(s => s.RecForFood).FirstOrDefault(x => x.Id == id);

            if (plan == null)
            {
                return NotFound();
            }

            List<Exercise> ex = new List<Exercise>();

            if(plan.Exercises != null)
            {
                foreach(var item in plan.Exercises)
                {
                    Exercise exvm = new Exercise()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        ImagePath = item.ImagePath,
                        NumberOfApproaches = item.NumberOfApproaches,
                        NumberOfRepetitions = item.NumberOfRepetitions,
                        Description = item.Description
                    };
                    ex.Add(exvm);
                }
            }

            RecForFood rec = plan.RecForFood;

            WorkoutPlanViewModel model = new WorkoutPlanViewModel()
            {
                Id = plan.Id,
                Name = plan.Name,
                Status = plan.Status,
                Description = plan.Description,
                Exercises = ex,
                RecForFood = rec
            };

            string oldImagePath = plan.ImagePath;

            List<RecForFood> recForFoodList = _context.RecForFoods.ToList();
            List<Exercise> exercisesList = _context.Exercises.ToList();

            ViewBag.RecForFoodList = recForFoodList;
            ViewBag.ExercisesList = exercisesList;
            ViewBag.OldImagePath = oldImagePath;

            return View(model);
        }

        // POST: WorkoutPlans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string OldImagePath, [Bind("Id,Name,Description,ImagePath,Status")] WorkoutPlanViewModel model)
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
                    if(model.ImagePath != null)
                    {
                        uniqueFileName = UploadedFile(model);

                        WorkoutPlan plan = new WorkoutPlan()
                        {
                            Id = model.Id,
                            Name = model.Name,
                            Description = model.Description,
                            Status = model.Status,
                            Exercises = model.Exercises,
                            RecForFood = model.RecForFood,
                            ImagePath = uniqueFileName
                        };
                        _context.Update(plan);
                    } else
                    {
                        WorkoutPlan plan = new WorkoutPlan()
                        {
                            Id = model.Id,
                            Name = model.Name,
                            Description = model.Description,
                            Status = model.Status,
                            Exercises = model.Exercises,
                            RecForFood = model.RecForFood,
                            ImagePath = OldImagePath
                        };
                        _context.Update(plan);
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkoutPlanExists(model.Id))
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

        // GET: WorkoutPlans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workoutPlan = await _context.WorkoutPlans
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workoutPlan == null)
            {
                return NotFound();
            }

            return View(workoutPlan);
        }

        // POST: WorkoutPlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workoutPlan = await _context.WorkoutPlans.FindAsync(id);
            _context.WorkoutPlans.Remove(workoutPlan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkoutPlanExists(int id)
        {
            return _context.WorkoutPlans.Any(e => e.Id == id);
        }

        private string UploadedFile(WorkoutPlanViewModel model)
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

        [HttpPost]
        public IActionResult DeleteRecForFoodFromWorkoutPlan(int? workoutplanid)
        {
            if(workoutplanid == null)
            {
                return NotFound();
            }
            WorkoutPlan plan = _context.WorkoutPlans.Include(c => c.RecForFood).FirstOrDefault(i => i.Id == workoutplanid);
            plan.RecForFood = null;
            _context.SaveChanges();

            string url = "Edit/" + workoutplanid;
            return Redirect(url);
        }

        [HttpPost]
        public IActionResult DeleteExerciseFromWorkoutPlan(int? workoutplanid, int? exerciseid)
        {
            if(workoutplanid == null)
            {
                return NotFound();
            }

            WorkoutPlan plan = _context.WorkoutPlans.Include(c => c.Exercises).FirstOrDefault(i => i.Id == workoutplanid);
            Exercise ex = _context.Exercises.Find(exerciseid);

            plan.Exercises.Remove(ex);
            _context.SaveChanges();

            string url = "Edit/" + workoutplanid;

            return Redirect(url);
        }

        [HttpPost]
        public IActionResult AddRecForFoodToWorkoutPlan(int? workoutplanid, int? recforfoodid)
        {
            if (workoutplanid == null)
            {
                return NotFound();
            }

            WorkoutPlan plan = _context.WorkoutPlans.Include(c => c.RecForFood).FirstOrDefault(i => i.Id == workoutplanid);
            RecForFood rec = _context.RecForFoods.Find(recforfoodid);
            plan.RecForFood = rec;
            _context.SaveChanges();

            string url = "Edit/" + workoutplanid;
            return Redirect(url);
        }

        [HttpPost]
        public IActionResult AddExerciseToWorkoutPlan(int? workoutplanid, int? exerciseid)
        {
            if (workoutplanid == null)
            {
                return NotFound();
            }

            WorkoutPlan plan = _context.WorkoutPlans.Include(c => c.Exercises).FirstOrDefault(i => i.Id == workoutplanid);
            Exercise ex = _context.Exercises.Find(exerciseid);

            plan.Exercises.Add(ex);
            _context.SaveChanges();

            string url = "Edit/" + workoutplanid;
            return Redirect(url);
        }
    }
}
