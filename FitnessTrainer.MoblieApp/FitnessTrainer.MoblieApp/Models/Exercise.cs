using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessTrainer.MoblieApp.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
        public int NumberOfApproaches { get; set; }
        public int NumberOfRepetitions { get; set; }
        public TimeSpan TimeBetweenSets { get; set; }
        public TimeSpan RestTimeAtEnd { get; set; }

        public List<WorkoutPlan> WorkoutPlans { get; set; }
    }
}
