using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FitnessTrainer.DomainEntities.Entity
{
    public class RecForFood
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Назва")]
        public string Name { get; set; }
        [Display(Name = "Опис")]
        public string Description { get; set; }

        public List<WorkoutPlan> WorkoutPlans { get; set; } = new List<WorkoutPlan>();
        public List<Food> Foods { get; set; } = new List<Food>();
    }
}
