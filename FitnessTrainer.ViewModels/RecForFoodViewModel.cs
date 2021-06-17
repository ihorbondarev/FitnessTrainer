using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FitnessTrainer.ViewModels;
using FitnessTrainer.DomainEntities.Entity;

namespace FitnessTrainer.ViewModels
{
    public class RecForFoodViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "RequiredDietNameError")]
        [Display(Name = "Назва")]
        public string Name { get; set; }
        [Display(Name = "Опис")]
        public string? Description { get; set; }
        public List<FoodViewModel> Foods { get; set; }
    }
}
