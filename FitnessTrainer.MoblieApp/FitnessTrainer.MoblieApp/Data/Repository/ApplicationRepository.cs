using System.Collections.Generic;
using SQLite;
using FitnessTrainer.MoblieApp.Data.Entity;

namespace FitnessTrainer.MoblieApp.Data.Repository
{
    public class ApplicationRepository
    {
        SQLiteConnection database;
        public ApplicationRepository(string databasePath)
        {
            database = new SQLiteConnection(databasePath);
            database.CreateTable<Food>();
            database.CreateTable<RecForFood>();
            database.CreateTable<Exercise>();
            database.CreateTable<WorkoutPlan>();
            database.CreateTable<ApplicationUser>();
            database.CreateTable<FoodAndRecForFood>();
            database.CreateTable<WorkoutPlanAndExercise>();
            database.CreateTable<ApplicationUserAndWorkoutPlan>();
        }

        //SQLiteConnection database;
        //public FriendRepository(string databasePath)
        //{
        //    database = new SQLiteConnection(databasePath);
        //    database.CreateTable<Friend>();
        //}
        //public IEnumerable<Friend> GetItems()
        //{
        //    return database.Table<Friend>().ToList();
        //}
        //public Friend GetItem(int id)
        //{
        //    return database.Get<Friend>(id);
        //}
        //public int DeleteItem(int id)
        //{
        //    return database.Delete<Friend>(id);
        //}
        //public int SaveItem(Friend item)
        //{
        //    if (item.Id != 0)
        //    {
        //        database.Update(item);
        //        return item.Id;
        //    }
        //    else
        //    {
        //        return database.Insert(item);
        //    }
        //}
    }
}
