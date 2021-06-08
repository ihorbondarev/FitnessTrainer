using FitnessTrainer.MoblieApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace FitnessTrainer.MoblieApp.ViewModels
{
    public class RegisterViewModel
    {
        private readonly ApiService _apiServices = new ApiService();
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Message { get; set; }
        public ICommand RegisterRequest
        {
            get
            {
                return new Command(async () =>
                {
                    var isRegistered = await _apiServices.RegisterUserAsync(UserName, Password, ConfirmPassword);

                    if (isRegistered)
                    {
                        Message = "Success register";
                    } else
                    {
                        Message = "Request failed";
                    }
                });
            }
        }
    }
}
