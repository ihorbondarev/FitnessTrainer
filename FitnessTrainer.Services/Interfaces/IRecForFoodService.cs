using FitnessTrainer.DomainEntities.Entity;
using FitnessTrainer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTrainer.Services.Interfaces
{
    public interface IRecForFoodService
    {
        Task<List<RecForFoodViewModel>> GetRecForFoods();
        Task<List<RecForFoodViewModel>> GetRecForFoods(string searchString);
        Task<RecForFoodViewModel> GetRecForFoodById(int? id);
        Task CreateRecForFood(RecForFood food);
        Task<RecForFoodViewModel> RecForFoodIntoViewModel(RecForFood food);
        Task UpdateRecForFood(RecForFood food);
        Task DeleteRecForFood(int id);
        Task AddFoodToRecForFood(int? idplan, int? idfood);
        Task DeleteFoodFromRecForFood(int? idplan, int? idfood);
    }
}
