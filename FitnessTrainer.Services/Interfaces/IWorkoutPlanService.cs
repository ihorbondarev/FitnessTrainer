using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitnessTrainer.DataAccess.DbContexts;
using FitnessTrainer.ViewModels;
using FitnessTrainer.DomainEntities.Entity;

namespace FitnessTrainer.Services.Interfaces
{
    public interface IWorkoutPlanService
    {
        Task<List<WorkoutPlan>> GetWorkoutPlans(string searchString);
        Task<WorkoutPlanViewModel> GetWorkoutPlanViewModelById(int? id);
        Task CreateWorkoutPlan(WorkoutPlanViewModel model, string uniqueFileName);
        Task UpdateWorkoutPlan(WorkoutPlanViewModel model, string uniqueFileName);
        Task UpdateWorkoutPlanWithOldImage(WorkoutPlanViewModel model, string OldImagePath);
        Task DeleteWorkoutPlanById(int id);
    }
}
