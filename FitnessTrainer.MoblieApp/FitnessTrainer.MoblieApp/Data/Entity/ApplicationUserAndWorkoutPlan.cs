using SQLite;
using SQLiteNetExtensions.Attributes;

namespace FitnessTrainer.MoblieApp.Data.Entity
{
    [Table("ApplicationUserAndWorkoutPlans")]
    public class ApplicationUserAndWorkoutPlan
    {
        [ForeignKey(typeof(ApplicationUser))]
        public int ApplacationUserId { get; set; }
        [ForeignKey(typeof(WorkoutPlan))]
        public int WorkoutPlanId { get; set; }
    }
}
