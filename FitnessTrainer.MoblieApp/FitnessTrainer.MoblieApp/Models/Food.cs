
using System.Collections.Generic;

namespace FitnessTrainer.MoblieApp.Models
{
    public class Food
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Calories { get; set; }
        public int Proteins { get; set; }
        public int Fats { get; set; }
        public int Carbohydrates { get; set; }

        public List<RecForFood> RecForFoods { get; set; } = new List<RecForFood>();
    }
}


//[Table("Friends")]
//public class Friend
//{
//    [PrimaryKey, AutoIncrement, Column("_id")]
//    public int Id { get; set; }

//    public string Name { get; set; }
//    public string Email { get; set; }
//    public string Phone { get; set; }
//}