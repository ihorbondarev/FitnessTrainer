using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace FitnessTrainer.ViewModels
{
    public class ExerciseViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле обов'язкове для заповнення.")]
        [Display(Name="Name")]
        public string Name { get; set; }
        [Display(Name = "Image")]
        public IFormFile ImagePath { get; set; }
        [Display(Name = "FileName")]
        public string ImagePathString { get; set; }
        [Display(Name = "Description")]
        public string? Description { get; set; }
        [Display(Name = "NumberOfApproaches")]
        public int? NumberOfApproaches { get; set; }
        [Display(Name = "NumberOfRepetitions")]
        public int? NumberOfRepetitions { get; set; }
        [Display(Name = "TimeBetweenSets")]
        public TimeSpan? TimeBetweenSets { get; set; }
        [Display(Name = "RestTimeAtTheEnd")]
        public TimeSpan? RestTimeAtTheEnd { get; set; }
    }
}
