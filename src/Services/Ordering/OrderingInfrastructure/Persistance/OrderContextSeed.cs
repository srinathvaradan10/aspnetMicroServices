using Microsoft.Extensions.Logging;
using OrderingDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingInfrastructure.Persistance
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
        {
            try
            {
                if (!orderContext.Orders.Any())
                {
                    orderContext.Orders.AddRange(GetPreconfiguredOrders());
                    await orderContext.SaveChangesAsync();
                    logger.LogInformation("Seed database associated with context {DbContextName}", typeof(OrderContext).Name);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message + ex.StackTrace);
            }
        }

        private static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return new List<Order>
            {
                new Order() {UserName = "swn", FirstName = "Srinath", LastName = "Varadan", EmailAddress = "srinathvaradan@gmail.com", AddressLine = "Chennai", Country = "India", State= "TamilNadu", ZipCode="2342342",CardName ="ABC",
                    CardNumber  = "1234", Expiration = "1/12", CVV = "321",PaymentMethod =1, TotalPrice = 350 }
            };
        }
    }
}
