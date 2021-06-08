using FitnessTrainer.MoblieApp.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace FitnessTrainer.MoblieApp.Views
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