using System;

namespace FitnessTrainer.DataAccess.DbContexts
{
    public class Databaseinitializer
    {
        public static void Init(IServiceProvider scopeServiceProvider)
        {
            //---------------------------------------------------------------------------------------------------------------
            //var userManager = scopeServiceProvider.GetService<UserManager<ApplicationUser>>();

            //var user = new ApplicationUser
            //{
            //    UserName = "Administrator"
            //};

            //var result = userManager.CreateAsync(user, "TestTest123").GetAwaiter().GetResult();
            //if (result.Succeeded)
            //{
            //    userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "Administrator")).GetAwaiter().GetResult();
            //}
            //---------------------------------------------------------------------------------------------------------------

            //context.Users.Add(user);
            //context.SaveChanges();
        }
    }
}
