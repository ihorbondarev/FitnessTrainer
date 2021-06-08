using System.ComponentModel.DataAnnotations;

namespace FitnessTrainer.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Поле логіну обов'язкове для заповнення.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Поле паролю обов'язкове для заповнення.")]
        public string Password { get; set; }

        //[Required]
        //public string ReturnUrl { get; set; }
    }
}
