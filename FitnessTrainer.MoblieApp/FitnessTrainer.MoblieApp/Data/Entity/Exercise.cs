using System;
using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace FitnessTrainer.MoblieApp.Data.Entity
{
    [Table("Exercises")]
    public class Exercise
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public int NumberOfApproaches { get; set; }
        public int NumberOfRepetitions { get; set; }
        public TimeSpan TimeBetweenSets { get; set; }
        public TimeSpan RestTimeAtEnd { get; set; }

        [ManyToMany(typeof(WorkoutPlanAndExercise))]
        public List<WorkoutPlan> WorkoutPlans { get; set; }
    }
}
