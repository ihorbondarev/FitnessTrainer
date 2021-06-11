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
using X.PagedList;
using FitnessTrainer.Services.Interfaces;

namespace FitnessTrainer.Controllers
{
    [Authorize(Policy = "Administrator")]
    public class WorkoutPlansController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IWorkoutPlanService _workoutPlanService;

        public WorkoutPlansController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment, IWorkoutPlanService workoutPlanService)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
            _workoutPlanService = workoutPlanService;
        }

        // GET: WorkoutPlans
        public async Task<IActionResult> Index(string searchString, int? page, int pageSize = 5)
        {
            ViewData["CurrentFilter"] = searchString;

            List<WorkoutPlan> plans = await _workoutPlanService.GetWorkoutPlans(searchString);

            int pageNumber = (page ?? 1);
            ViewBag.workoutPlansList = plans.ToPagedList(pageNumber, pageSize);

            return View(plans);
        }

        // GET: WorkoutPlans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            WorkoutPlanViewModel model = await _workoutPlanService.GetWorkoutPlanViewModelById(id);


            if (model == null)
            {
                return NotFound();
            }

            List<Exercise> exlist = model.Exercises;

            ViewBag.Exercises = exlist;

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

                await _workoutPlanService.CreateWorkoutPlan(model, uniqueFileName);

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

            WorkoutPlanViewModel model = await _workoutPlanService.GetWorkoutPlanViewModelById(id);

            List<RecForFood> recForFoodList = _context.RecForFoods.ToList();
            List<Exercise> exercisesList = _context.Exercises.ToList();

            ViewBag.RecForFoodList = recForFoodList;
            ViewBag.ExercisesList = exercisesList;

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

                        await _workoutPlanService.UpdateWorkoutPlan(model, uniqueFileName);
                    } else
                    {
                        await _workoutPlanService.UpdateWorkoutPlanWithOldImage(model, OldImagePath);
                    }
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

            WorkoutPlanViewModel model = await _workoutPlanService.GetWorkoutPlanViewModelById(id);
            
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: WorkoutPlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _workoutPlanService.DeleteWorkoutPlanById(id);
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
