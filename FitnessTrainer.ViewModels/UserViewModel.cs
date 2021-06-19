using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitnessTrainer.DomainEntities.Entity;
using System.ComponentModel.DataAnnotations;

namespace FitnessTrainer.ViewModels
{
    public class UserViewModel
    {
        [Display(Name = "GuidIdentificator")]
        public Guid Id { get; set; }
        [Display(Name = "UserNameViewModel")]
        public string Name { get; set; }
        [Display(Name = "UserWeightViewModel")]
        public int? Weight { get; set; }
        [Display(Name = "UserHeightViewModel")]
        public int? Height { get; set; }
        [Display(Name = "UserAgeViewModel")]
        public int? Age { get; set; }
        [Display(Name = "UserSubscriptionViewModel")]
        public Status Subscription { get; set; }
        [Display(Name = "UserWorkoutPlansViewModel")]

        public List<WorkoutPlan> WorkoutPlans { get; set; }
    }
}
