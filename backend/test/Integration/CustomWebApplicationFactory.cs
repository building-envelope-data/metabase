using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ApplicationDbContext = Icon.Data.ApplicationDbContext;
using Microsoft.AspNetCore.Identity;
using User = Icon.Domain.User;

namespace Test.Integration.Web.Api
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Create a new service provider.
                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                // Add a database context using an in-memory database for testing.
                services.AddDbContext<ApplicationDbContext>(_ =>
                {
                    _.UseInMemoryDatabase("InMemoryAppDb");
                    _.UseInternalServiceProvider(serviceProvider);
                });

                // Build the service provider.
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database contexts
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var applicationDbContext = scopedServices.GetRequiredService<ApplicationDbContext>();
                    var userManager = scopedServices.GetRequiredService<UserManager<User>>();

                    var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                    // Ensure the database is created.
                    applicationDbContext.Database.EnsureCreated();

                    try
                    {
                        // Seed the database with test users.
                        SeedData.PopulateUsers(userManager);
                        // Seed the database with test data.
                        SeedData.PopulateTestData(applicationDbContext);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the " +
                                            "database with test messages. Error: {ex.Message}");
                    }
                }
            });
        }
    }
}