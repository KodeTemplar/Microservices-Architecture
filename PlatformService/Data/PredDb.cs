using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using PlatformService.Models;

namespace PlatformService.Data
{
    public class PredDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<ApplicationDbContext>());
            }
        }

        private static void SeedData(ApplicationDbContext context)
        {
            //context.Database.EnsureCreated();
            Console.WriteLine("---> Checking if Db exist");
            var exist = (context.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists();

            if (!exist)
            {
                Console.WriteLine("---> Running Migration");
                context.Database.Migrate();
                exist = true;
            }
            if (exist)
            {
                if (!context.Platforms.Any())
                {
                    Console.WriteLine("--->Seeding data ....");

                    context.Platforms.AddRange
                        (
                        new Platform() { Name = "Dot Net", Publisher = "Microsoft", Cost = "Free" },
                        new Platform() { Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free" },
                        new Platform() { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
                        );
                    context.SaveChanges();
                    Console.WriteLine("---> Migration Completed successfully");
                }
                else
                {
                    Console.WriteLine("---> We already have a data");
                }
            }
        }
    }
}
