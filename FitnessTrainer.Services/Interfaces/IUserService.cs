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
    public interface IUserService
    {
        Task<UserViewModel> GetUserViewModelById(Guid? id);
        Task<bool> AddWorkoutPlanToUserPlans(Guid? id, int? planid);
        Task<bool> DeletePlanFromUsersPlans(Guid? userid, int? planid);
        Task<bool> ApplyPremiumStatusForUser(Guid? userid);
        Task<bool> EditUserProfile(ApplicationUser user);
    }
}
