using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace FitnessTrainer.DomainEntities.Entity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [Required]
        [Display(Name = "Підписка")]
        public bool Subscription { get; set; }
        [Display(Name = "Вік")]
        public int Age { get; set; }
        [Display(Name = "Вага")]
        public int Weight { get; set; }
        [Display(Name = "Зріст")]
        public int Height { get; set; }

        public List<WorkoutPlan> Plans { get; set; }
    }
}
