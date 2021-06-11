using FitnessTrainer.Services.Interfaces;
using FitnessTrainer.DataAccess.DbContexts;
using FitnessTrainer.DomainEntities.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitnessTrainer.ViewModels;

namespace FitnessTrainer.Services
{
    public class FoodService : IFoodService
    {
        private readonly ApplicationDbContext _context;

        public FoodService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Food>> GetFood()
        {
            List<Food> foodList = _context.Foods.ToList();
            return foodList;
        }

        public async Task<List<Food>> GetFood(string searchString)
        {
            List<Food> food = _context.Foods.ToList();
            if(!String.IsNullOrEmpty(searchString))
            {
                food = _context.Foods.Where(c => c.Name.Contains(searchString)).ToList();
            }
            return food;
        }

        public async Task<FoodViewModel> GetFoodById(int? id)
        {
            Food food = _context.Foods.FirstOrDefault(m => m.Id == id);
            FoodViewModel model = new FoodViewModel()
            {
                Id = food.Id,
                Name = food.Name,
                Calories = food.Calories,
                Carbohydrates = food.Carbohydrates,
                Fats = food.Fats,
                Proteins = food.Proteins
            };
            return model;
        }

        public async Task CreateFood(Food model)
        {
            _context.Add(model);
            _context.SaveChanges();
        }

        public async Task UpdateFood(Food model)
        {
            _context.Update(model);
            _context.SaveChanges();
        }

        public async Task<FoodViewModel> FoodIntoViewModel(Food food)
        {
            FoodViewModel model = new FoodViewModel()
            {
                Id = food.Id,
                Name = food.Name,
                Calories = food.Calories,
                Carbohydrates = food.Carbohydrates,
                Fats = food.Fats,
                Proteins = food.Proteins
            };
            return model;
        }

        public async Task DeleteFood(int id)
        {
            Food food = _context.Foods.Find(id);
            _context.Foods.Remove(food);
            _context.SaveChanges();
        }
    }
}
