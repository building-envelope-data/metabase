using System;
using Icon.Domain;
using ApplicationDbContext = Icon.Data.ApplicationDbContext;
using Microsoft.AspNetCore.Identity;
using User = Icon.Domain.User;

namespace Test.Integration.Web.Api
{
    public static class SeedData
    {
        public static void PopulateUsers(UserManager<User> userManager)
        {
            userManager.CreateAsync(
              new User()
              {
                  /* Id = 1, */
                  Email = "simon@icon.com"
                  /* Created = DateTime.UtcNow, */
              },
              "simonSIMON123@"
              );
        }

        public static void PopulateTestData(ApplicationDbContext dbContext)
        {
        }
    }
}