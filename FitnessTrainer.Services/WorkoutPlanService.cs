using FitnessTrainer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitnessTrainer.ViewModels;
using FitnessTrainer.DomainEntities.Entity;
using FitnessTrainer.DataAccess.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace FitnessTrainer.Services
{
    public class WorkoutPlanService : IWorkoutPlanService
    {
        private readonly ApplicationDbContext _context;

        public WorkoutPlanService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<WorkoutPlan>> GetWorkoutPlans(string searchString)
        {
            List<WorkoutPlan> plans = _context.WorkoutPlans.ToList();

            if(!String.IsNullOrEmpty(searchString))
            {
                plans = _context.WorkoutPlans.Where(c => c.Name.Contains(searchString)).ToList();
            }

            return plans;
        }
        public async Task<WorkoutPlanViewModel> GetWorkoutPlanViewModelById(int? id)
        {
            WorkoutPlan plan = _context.WorkoutPlans.Include(c => c.Exercises).Include(v => v.RecForFood).FirstOrDefault(i => i.Id == id);

            WorkoutPlanViewModel model = new WorkoutPlanViewModel()
            {
                Id = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                Status = plan.Status,
                RecForFood = plan.RecForFood,
                ImagePathString = plan.ImagePath,
                Exercises = plan.Exercises
            };
            return model;
        }
        public async Task CreateWorkoutPlan(WorkoutPlanViewModel model, string uniqueFileName)
        {
            WorkoutPlan plan = new WorkoutPlan()
            {
                Name = model.Name,
                Description = model.Description,
                ImagePath = uniqueFileName,
                Status = model.Status
            };

            _context.Add(plan);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateWorkoutPlan(WorkoutPlanViewModel model, string uniqueFileName)
        {
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
            await _context.SaveChangesAsync();
        }
        public async Task UpdateWorkoutPlanWithOldImage(WorkoutPlanViewModel model, string OldImagePath)
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
            await _context.SaveChangesAsync();
        }
        public async Task DeleteWorkoutPlanById(int id)
        {
            WorkoutPlan plan = _context.WorkoutPlans.Find(id);
            _context.WorkoutPlans.Remove(plan);
            _context.SaveChanges();
        }
    }
}
