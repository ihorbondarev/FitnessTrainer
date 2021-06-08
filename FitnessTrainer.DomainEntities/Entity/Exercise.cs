using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FitnessTrainer.DomainEntities.Entity
{
    public class Exercise
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Назва")]
        public string Name { get; set; }
        [Display(Name = "Зображення")]
        public string ImagePath { get; set; }
        [Display(Name = "Опис")]
        public string? Description { get; set; }
        [Display(Name = "Кількість підходів")]
        public int? NumberOfApproaches { get; set; }
        [Display(Name = "Кількість повторів")]
        public int? NumberOfRepetitions { get; set; }
        [Display(Name = "Перерва між підходами")]
        public TimeSpan? TimeBetweenSets { get; set; }
        [Display(Name = "Перерва після вправи")]
        public TimeSpan? RestTimeAtTheEnd { get; set; }

        public List<WorkoutPlan> Plans { get; set; }
    }
}
