using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessTrainer.ViewModels
{
    public class FoodViewModel
    {
        public int Id { get; set; }
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
    }
}