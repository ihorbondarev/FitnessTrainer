using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace FitnessTrainer.ViewModels
{
    public class ExerciseViewModel
    {
        public int Id { get; set; }
        [Display(Name="Название")]
        [Required]
        public string Name { get; set; }
        [Display(Name = "Изображение")]
        public IFormFile ImagePath { get; set; }
        [Display(Name = "Назва файлу")]
        public string ImagePathString { get; set; }
        [Display(Name = "Описание")]
        public string? Description { get; set; }
        [Display(Name = "Кол. подходов")]
        public int? NumberOfApproaches { get; set; }
        [Display(Name = "Кол. повторений")]
        public int? NumberOfRepetitions { get; set; }
        [Display(Name = "Отдых между подходами")]
        public TimeSpan? TimeBetweenSets { get; set; }
        [Display(Name = "Отдых после упражнения")]
        public TimeSpan? RestTimeAtTheEnd { get; set; }
    }
}
