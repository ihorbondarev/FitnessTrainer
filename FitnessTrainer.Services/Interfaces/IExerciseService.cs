using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitnessTrainer.DomainEntities.Entity;
using FitnessTrainer.ViewModels;
using FitnessTrainer.DataAccess.DbContexts;

namespace FitnessTrainer.Services.Interfaces
{
    public interface IExerciseService
    {
        Task<List<ExerciseViewModel>> GetExercises(string searchString);
        Task<List<Exercise>> GetExercisesInDomainFormat(string searchString);
        Task<ExerciseViewModel> GetExerciseById(int? id);
        Task UpdateExercise(ExerciseViewModel model, string uniqueFileName);
        Task UpdateExerciseWithOldImage(ExerciseViewModel model, string OldImagePath);
        Task CreateExercise(ExerciseViewModel model, string uniqueFileName);
        Task DeleteExercise(int id);
    }
}
