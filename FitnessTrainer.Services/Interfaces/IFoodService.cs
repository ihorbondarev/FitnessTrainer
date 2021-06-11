using FitnessTrainer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitnessTrainer.DomainEntities.Entity;

namespace FitnessTrainer.Services.Interfaces
{
    public interface IFoodService
    {
        Task<List<Food>> GetFood();
        Task<List<Food>> GetFood(string searchString);
        Task<FoodViewModel> GetFoodById(int? id);
        Task CreateFood(Food food);
        Task UpdateFood(Food food);
        Task<FoodViewModel> FoodIntoViewModel(Food food);
        Task DeleteFood(int id);
    }
}
