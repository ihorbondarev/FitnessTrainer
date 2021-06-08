using SQLite;
using SQLiteNetExtensions.Attributes;

namespace FitnessTrainer.MoblieApp.Data.Entity
{
    [Table("FoodAndRecForFoods")]
    public class FoodAndRecForFood
    {
        [ForeignKey(typeof(RecForFood))]
        public int RecForFoodId { get; set; }
        
        [ForeignKey(typeof(Food))]
        public int FoodId { get; set; }
    }
}
