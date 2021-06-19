using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace FitnessTrainer.DomainEntities.Entity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [Required]
        public Status Subscription { get; set; }
        public int? Age { get; set; }
        public int? Weight { get; set; }
        public int? Height { get; set; }

        public List<WorkoutPlan> Plans { get; set; } = new List<WorkoutPlan>();
    }
}
