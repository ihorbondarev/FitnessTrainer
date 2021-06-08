using FitnessTrainer.MoblieApp.Services;
using System;
using System.IO;
using Xamarin.Forms;
using FitnessTrainer.MoblieApp.Data.Repository;

namespace FitnessTrainer.MoblieApp
{
    public partial class App : Application
    {

        public const string DATABASE_NAME = "FitnessTrainer.db";
        public static ApplicationRepository database;
        public static ApplicationRepository Database
        {
            get
            {
                if (database == null)
                {
                    database = new ApplicationRepository(
                        Path.Combine(
                            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DATABASE_NAME));
                }
                return database;
            }
        }

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
