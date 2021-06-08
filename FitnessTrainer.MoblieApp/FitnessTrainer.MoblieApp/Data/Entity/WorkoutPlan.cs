using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace FitnessTrainer.MoblieApp.Data.Entity
{
    [Table("WorkoutPlans")]
    public class WorkoutPlan
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public Status Status { get; set; }

        [ForeignKey(typeof(RecForFood))]
        public int RecForFoodId { get; set; }

        [ManyToMany(typeof(WorkoutPlanAndExercise))]
        public List<Exercise> Exercises { get; set; }
        [ManyToMany(typeof(ApplicationUserAndWorkoutPlan))]
        public List<ApplicationUser> ApplicationUsers { get; set; }
    }
}
