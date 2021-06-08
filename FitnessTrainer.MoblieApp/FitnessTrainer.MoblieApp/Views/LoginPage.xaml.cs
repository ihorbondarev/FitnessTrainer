using FitnessTrainer.MoblieApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FitnessTrainer.MoblieApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = new LoginViewModel();
        }

        private async void routeToRegisterPage(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new CommonPage());
            await Navigation.PushAsync(new RegisterPage());
        }
    }
}