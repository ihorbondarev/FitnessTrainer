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
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserViewModel> GetUserViewModelById(Guid? id)
        {
            if(id == null)
            {
                return null;
            }

            ApplicationUser user = _context.ApplicationUsers.Include(c => c.Plans).FirstOrDefault(c => c.Id == id);

            var model = new UserViewModel()
            {
                Id = user.Id,
                Name = user.UserName,
                Age = user.Age,
                Weight = user.Weight,
                Height = user.Height,
                Subscription = user.Subscription,
                WorkoutPlans = user.Plans
            };

            return model;
        }

        public async Task<bool> AddWorkoutPlanToUserPlans(Guid? userid, int? planid)
        {
            if(userid == null || planid == null)
            {
                return false;
            }

            ApplicationUser user = await _context.ApplicationUsers.Include(w => w.Plans).FirstOrDefaultAsync(c => c.Id == userid);
            WorkoutPlan plan = await _context.WorkoutPlans.FindAsync(planid);

            if(plan.Status == Status.Premium)
            {
                if(user.Subscription != Status.Premium)
                {
                    return false;
                }
            }

            user.Plans.Add(plan);
            _context.SaveChanges();

            return true;
        }

        public async Task<bool> DeletePlanFromUsersPlans(Guid? userid, int? planid)
        {
            if (userid == null || planid == null)
            {
                return false;
            }

            ApplicationUser user = await _context.ApplicationUsers.Include(w => w.Plans).FirstOrDefaultAsync(c => c.Id == userid);
            WorkoutPlan plan = await _context.WorkoutPlans.FindAsync(planid);

            user.Plans.Remove(plan);
            _context.SaveChanges();

            return true;
        }

        public async Task<bool> ApplyPremiumStatusForUser(Guid? userid)
        {
            if(userid == null)
            {
                return false;
            }

            ApplicationUser user = await _context.ApplicationUsers.FirstOrDefaultAsync(c => c.Id == userid);

            user.Subscription = Status.Premium;

            _context.SaveChanges();

            return true;
        }

        public async Task<bool> EditUserProfile(ApplicationUser user)
        {
            ApplicationUser model = await _context.ApplicationUsers.FirstOrDefaultAsync(c => c.Id == user.Id);

            if(model == null)
            {
                return false;
            }

            model.Age = user.Age;
            model.Weight = user.Weight;
            model.Height = user.Height;

            _context.SaveChanges();

            return true;
        }
    }
}
