using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace FitnessTrainer.MoblieApp.Data.Entity
{
    [Table("RecForFoods")]
    public class RecForFood
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [ManyToMany(typeof(FoodAndRecForFood))]
        public List<Food> Foods { get; set; }

        [OneToMany]
        public List<WorkoutPlan> WorkoutPlans { get; set; }
    }
}
