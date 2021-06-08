using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace FitnessTrainer.MoblieApp.Data.Entity
{
    [Table("ApplicationUsers")]
    public class ApplicationUser
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }

        [ManyToMany(typeof(ApplicationUserAndWorkoutPlan))]
        public List<WorkoutPlan> WorkoutPlans { get; set; }
    }
}
