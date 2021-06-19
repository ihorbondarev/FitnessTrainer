using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FitnessTrainer.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "RegisterUserNameRequiredError")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "RegisterPasswordRequiredError")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "ConfirmPasswordRequiredError")]
        [Compare("Password", ErrorMessage = "ComparePasswordsErrorMessage")]
        public string ConfirmPassword { get; set; }
    }
}
