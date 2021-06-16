using FitnessTrainer.Services.Interfaces;
using FitnessTrainer.DomainEntities.Entity;
using FitnessTrainer.DataAccess.DbContexts;
using System;
using FitnessTrainer.ViewModels;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FitnessTrainer.Services
{
    public class ExerciseService : IExerciseService
    {
        private readonly ApplicationDbContext _context;

        public ExerciseService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ExerciseViewModel>> GetExercises(string searchString)
        {
            List<Exercise> exercises = _context.Exercises.ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                exercises = _context.Exercises.Where(c => c.Name.Contains(searchString)).ToList();
            }

            List<ExerciseViewModel> model = new List<ExerciseViewModel>();

            foreach(var i in exercises)
            {
                ExerciseViewModel ex = new ExerciseViewModel()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Description = i.Description,
                    ImagePathString = i.ImagePath,
                    NumberOfApproaches = i.NumberOfApproaches,
                    NumberOfRepetitions = i.NumberOfRepetitions,
                    TimeBetweenSets = i.TimeBetweenSets,
                    RestTimeAtTheEnd = i.RestTimeAtTheEnd
                };
                model.Add(ex);
            }
            return model;
        }
        public async Task<List<Exercise>> GetExercisesInDomainFormat(string searchString)
        {
            List<Exercise> ex = _context.Exercises.ToList();
            if(!String.IsNullOrEmpty(searchString))
            {
                ex = _context.Exercises.Where(c => c.Name.Contains(searchString)).ToList();
            }
            return ex;
        }
        public async Task<ExerciseViewModel> GetExerciseById(int? id)
        {
            Exercise ex = _context.Exercises.Find(id);
            if (ex == null)
            {
                return null;
            }
            ExerciseViewModel model = new ExerciseViewModel()
            {
                Id = ex.Id,
                Name = ex.Name,
                Description = ex.Description,
                NumberOfApproaches = ex.NumberOfApproaches,
                NumberOfRepetitions = ex.NumberOfRepetitions,
                TimeBetweenSets = ex.TimeBetweenSets,
                RestTimeAtTheEnd = ex.RestTimeAtTheEnd,
                ImagePathString = ex.ImagePath
            };
            return model;
        }
        public async Task UpdateExercise(ExerciseViewModel model, string uniqueFileName)
        {
            Exercise ex = new Exercise()
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
            _context.Update(ex);
            _context.SaveChanges();
        }
        public async Task UpdateExerciseWithOldImage(ExerciseViewModel model, string OldImagePath)
        {
            Exercise ex = new Exercise()
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
            _context.Update(ex);
            _context.SaveChanges();
        }
        public async Task CreateExercise(ExerciseViewModel model, string uniqueFileName)
        {
            Exercise exercise = new Exercise();
            if (model.Id != null)
            {
                exercise = new Exercise()
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
            } else
            {
                exercise = new Exercise()
                {
                    Name = model.Name,
                    ImagePath = uniqueFileName,
                    Description = model.Description,
                    NumberOfApproaches = model.NumberOfApproaches,
                    NumberOfRepetitions = model.NumberOfRepetitions,
                    TimeBetweenSets = model.TimeBetweenSets,
                    RestTimeAtTheEnd = model.RestTimeAtTheEnd
                };
            }
            
            _context.Add(exercise);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteExercise(int id)
        {
            Exercise exercise = _context.Exercises.Find(id);
            _context.Exercises.Remove(exercise);
            await _context.SaveChangesAsync();
        }
    }


}
