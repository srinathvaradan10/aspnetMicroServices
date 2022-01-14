﻿
using Microsoft.EntityFrameworkCore;
using OrderingInfrastructure.Persistance;

namespace OrderingAPI.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder, int? retry = 0)
            where TContext : DbContext
        {
            int retryValue = retry.Value;

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var config = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetService<TContext>();

                try
                {
                    logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);

                    InvokeSeeder(seeder, context, services);

                    logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);


                }
                catch (Exception ex)
                {

                    logger.LogError(ex, "An error occurred while migrating the postresql database");

                    if (retryValue < 50)
                    {
                        retryValue++;
                        Thread.Sleep(2000);
                        MigrateDatabase(host, seeder, retryValue);
                    }
                }
                return host;
            }

        }


        private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder,
                                                   TContext context,
                                                   IServiceProvider services)
                                                   where TContext : DbContext
        {
            context.Database.Migrate();
            seeder(context, services);
        }
    }
}
