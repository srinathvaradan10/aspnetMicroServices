using OrderingDomain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderingApplication.Contracts.Persistence
{
    public interface IOrderRepository : IAsyncRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByUserName(string userName);
    }
}
