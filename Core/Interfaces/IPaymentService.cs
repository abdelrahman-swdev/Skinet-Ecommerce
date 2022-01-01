using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAggregate;

namespace Core.Interfaces
{
    public interface IPaymentService
    {
        Task<CustomerBasket> SavePaymentIntentAsync(string basketId);
        Task<Order> UpdateOrderPaymentSucceeded(string intentId);
        Task<Order> UpdateOrderPaymentFailed(string intentId);
    }
}