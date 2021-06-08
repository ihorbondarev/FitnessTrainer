using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FitnessTrainer.DomainEntities.Entity
{
    public class Food
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Назва")]
        public string Name { get; set; }
        [Display(Name = "Калорії")]
        public int Calories { get; set; }
        [Display(Name = "Білки")]
        public int Proteins { get; set; }
        [Display(Name = "Жири")]
        public int Fats { get; set; }
        [Display(Name = "Вуглеводи")]
        public int Carbohydrates { get; set; }

        public List<RecForFood> RecForFoods { get; set; } = new List<RecForFood>();
    }
}
