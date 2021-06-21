using FitnessTrainer.DataAccess.DbContexts;
using FitnessTrainer.DomainEntities.Entity;
using FitnessTrainer.Services.Interfaces;
using FitnessTrainer.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTrainer.Services
{
    public class RecForFoodService : IRecForFoodService
    {
        private readonly ApplicationDbContext _context;

        public RecForFoodService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<RecForFoodViewModel>> GetRecForFoods()
        {
            List<RecForFood> recForFoodList = _context.RecForFoods.ToList();
            List<RecForFoodViewModel> recVMList = new List<RecForFoodViewModel>();
            foreach (var item in recForFoodList)
            {
                RecForFoodViewModel vm = new RecForFoodViewModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description
                };
                recVMList.Add(vm);
            }

            return recVMList;
        }

        public async Task<List<RecForFoodViewModel>> GetRecForFoods(string searchString)
        {
            List<RecForFood> recForFoodList = _context.RecForFoods.ToList();
            if(!String.IsNullOrEmpty(searchString))
            {
                recForFoodList = _context.RecForFoods.Where(c => c.Name.Contains(searchString)).ToList();
            }
            List<RecForFoodViewModel> recVMList = new List<RecForFoodViewModel>();
            foreach (var item in recForFoodList)
            {
                RecForFoodViewModel vm = new RecForFoodViewModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description
                };
                recVMList.Add(vm);
            }

            return recVMList;
        }

        public async Task<RecForFoodViewModel> GetRecForFoodById(int? id)
        {
            RecForFood recForFood = _context.RecForFoods.Include(c => c.Foods).FirstOrDefault(i => i.Id == id);

            List<FoodViewModel> VMList = new List<FoodViewModel>();

            if (recForFood.Foods != null)
            {
                List<Food> foodList = recForFood.Foods;

                foreach (var i in foodList)
                {
                    FoodViewModel vm = new FoodViewModel()
                    {
                        Id = i.Id,
                        Name = i.Name,
                        Calories = i.Calories,
                        Fats = i.Fats,
                        Proteins = i.Proteins,
                        Carbohydrates = i.Carbohydrates
                    };
                    VMList.Add(vm);
                }
            }

            RecForFoodViewModel model = new RecForFoodViewModel()
            {
                Id = recForFood.Id,
                Name = recForFood.Name,
                Description = recForFood.Description
            };

            if(VMList != null)
            {
                model.Foods = VMList;
            }

            return model;
        }
        
        public async Task CreateRecForFood(RecForFood food)
        {
            _context.Add(food);
            _context.SaveChanges();
        }

        public async Task<RecForFoodViewModel> RecForFoodIntoViewModel(RecForFood food)
        {
            RecForFoodViewModel model = new RecForFoodViewModel()
            {
                Id = food.Id,
                Name = food.Name,
                Description = food.Description
            };
            return model;
        }

        public async Task UpdateRecForFood(RecForFood food)
        {
            _context.Update(food);
            _context.SaveChanges();
        }

        public async Task DeleteRecForFood(int id)
        {
            RecForFood model = _context.RecForFoods.Find(id);

            List<WorkoutPlan> planList = _context.WorkoutPlans.Include(c => c.RecForFood).Where(i => i.RecForFood.Id == id).ToList();

            foreach(var rec in planList)
            {
                _context.RecForFoods.Remove(model);
            }

            _context.RecForFoods.Remove(model);
            _context.SaveChanges();
        }

        public async Task AddFoodToRecForFood(int? planid, int? foodid)
        {
            RecForFood recPlan = _context.RecForFoods.Find(planid);
            Food food = _context.Foods.Find(foodid);
            recPlan.Foods.Add(food);
            _context.SaveChanges();
        }

        public async Task DeleteFoodFromRecForFood(int? idplan, int? idfood)
        {
            RecForFood rec = _context.RecForFoods.Include(c => c.Foods).FirstOrDefault(i => i.Id == idplan);
            Food food = _context.Foods.Find(idfood);
            rec.Foods.Remove(food);
            _context.SaveChanges();
        }
    }
}
