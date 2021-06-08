using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace FitnessTrainer.MoblieApp.Data.Entity
{
    [Table("Foods")]
    public class Food
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }
        public int Calories { get; set; }
        public int Proteins { get; set; }
        public int Fats { get; set; }
        public int Carbohydrates { get; set; }

        [ManyToMany(typeof(FoodAndRecForFood))]
        public List<RecForFood> RecForFoods { get; set; }
    }
}
