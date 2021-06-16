using FitnessTrainer.MobileApp.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace FitnessTrainer.MobileApp.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}