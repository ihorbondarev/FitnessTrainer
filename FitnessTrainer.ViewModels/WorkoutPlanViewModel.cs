using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FitnessTrainer.DomainEntities.Entity;
using Microsoft.AspNetCore.Http;

namespace FitnessTrainer.ViewModels
{
    public class WorkoutPlanViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "RequiredWorkoutPlanNameError")]
        [Display(Name = "Назва")]
        public string Name { get; set; }
        [Display(Name = "Опис")]
        public string? Description { get; set; }
        [Display(Name = "Назва файлу")]
        public string ImagePathString { get; set; }
        [Display(Name = "Зображення")]
        public IFormFile ImagePath { get; set; }
        [Display(Name = "Статус")]
        public Status Status { get; set; }
        [Display(Name = "План харчування")]
        public RecForFood RecForFood { get; set; }
        public List<Exercise> Exercises { get; set; }

    }
}
