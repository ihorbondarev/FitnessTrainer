using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using FitnessTrainer.MoblieApp.Models;

namespace FitnessTrainer.MoblieApp
{
    class ApplicationContext : DbContext
    {
        private string _databasePath;

        public virtual DbSet<Exercise> Exercises { get; set; }
        public virtual DbSet<WorkoutPlan> WorkoutPlans { get; set; }
        public virtual DbSet<Food> Foods { get; set; }
        public virtual DbSet<RecForFood> RecForFoods { get; set; }
        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public ApplicationContext(string databasePath)
        {
            _databasePath = databasePath;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={_databasePath}");
        }
    }
}
