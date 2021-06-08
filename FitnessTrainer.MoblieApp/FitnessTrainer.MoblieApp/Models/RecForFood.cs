using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessTrainer.MoblieApp.Models
{
    public class RecForFood
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<WorkoutPlan> WorkoutPlans { get; set; } = new List<WorkoutPlan>();
        public List<Food> Foods { get; set; } = new List<Food>();
    }
}
