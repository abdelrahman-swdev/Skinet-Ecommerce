using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IBasketRepository
    {
        Task<CustomerBasket> GetBasketAsync(string BasketId);
        Task<CustomerBasket> SaveBasketAsync(CustomerBasket basket);
        Task<bool> DeleteBasketAsync(string BasketId);
    }
}