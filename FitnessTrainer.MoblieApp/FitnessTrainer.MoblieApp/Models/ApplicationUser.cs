using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessTrainer.MoblieApp.Models
{
    public class ApplicationUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public bool Subscription { get; set; }
        public int Age { get; set; }

        public List<WorkoutPlan> WorkoutPlans { get; set; } = new List<WorkoutPlan>();
    }
}
