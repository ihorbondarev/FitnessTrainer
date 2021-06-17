using System.ComponentModel.DataAnnotations;

namespace FitnessTrainer.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "RequiredLoginError")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "RequiredPasswordError")]
        public string Password { get; set; }

        //[Required]
        //public string ReturnUrl { get; set; }
    }
}
