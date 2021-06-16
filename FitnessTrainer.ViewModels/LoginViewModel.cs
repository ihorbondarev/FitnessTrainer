using System.ComponentModel.DataAnnotations;

namespace FitnessTrainer.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введіть логін.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Введіть пароль.")]
        public string Password { get; set; }

        //[Required]
        //public string ReturnUrl { get; set; }
    }
}
