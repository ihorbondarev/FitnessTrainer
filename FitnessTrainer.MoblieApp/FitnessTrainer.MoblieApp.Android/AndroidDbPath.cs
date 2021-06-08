using System;
using FitnessTrainer.MoblieApp.Droid;
using FitnessTrainer.MoblieApp;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidDbPath))]
namespace FitnessTrainer.MoblieApp.Droid
{
    public class AndroidDbPath
    {
        public string GetDatabasePath(string filename)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), filename);
        }
    }
}