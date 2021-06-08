using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FitnessTrainer.DomainEntities.Entity
{
    public class WorkoutPlan
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }

        [Required]
        public Status Status { get; set; }

        public RecForFood RecForFood { get; set; }
        public List<Exercise> Exercises { get; set; } = new List<Exercise>();
        public List<ApplicationUser> ApplicationUsers { get; set; } = new List<ApplicationUser>();
    }
}
