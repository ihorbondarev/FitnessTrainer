using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessTrainer.MoblieApp.Models
{
    public class WorkoutPlan
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }

        public Status Status { get; set; }

        public RecForFood RecForFood { get; set; }
        public List<Exercise> Exercises { get; set; }
    }
}
