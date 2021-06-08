using SQLite;
using SQLiteNetExtensions.Attributes;

namespace FitnessTrainer.MoblieApp.Data.Entity
{
    [Table("WorkoutPlanAndExercises")]
    public class WorkoutPlanAndExercise
    {
        [ForeignKey(typeof(WorkoutPlan))]
        public int WorkoutPlanId { get; set; }
        [ForeignKey(typeof(Exercise))]
        public int ExerciseId { get; set; }
    }
}
