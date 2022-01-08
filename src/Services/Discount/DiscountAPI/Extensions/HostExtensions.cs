using Npgsql;

namespace DiscountAPI.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, int? retry = 0)
        {
            int retryValue = retry.Value;

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var config = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<TContext>>();

                try
                {
                    logger.LogInformation("Migrating postgres");
                    using(var con = new NpgsqlConnection(config.GetValue<string>("DatabaseSettings:ConnectionString")))
                    {
                        con.Open();
                        using (var command = new NpgsqlCommand()
                        {
                            Connection = con,
                        })
                        {
                            command.CommandText = "DROP TABLE IF EXISTS COUPON";
                            command.ExecuteNonQuery();
                            command.CommandText = @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY, 
                                                                ProductName VARCHAR(24) NOT NULL,
                                                                Description TEXT,
                                                                Amount INT)";
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('IPhone X', 'IPhone Discount', 150);";
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Samsung 10', 'Samsung Discount', 100);";
                            command.ExecuteNonQuery();

                            logger.LogInformation(con.ConnectionString + "Migrated postresql database.");
                        }
                    }
                }
                catch (Exception ex)
                {

                    logger.LogError(ex, "An error occurred while migrating the postresql database");

                    if (retryValue < 50)
                    {
                        retryValue++;
                        System.Threading.Thread.Sleep(2000);
                        MigrateDatabase<TContext>(host, retryValue);
                    }
                }
                return host;
            }

        }
    }
}
