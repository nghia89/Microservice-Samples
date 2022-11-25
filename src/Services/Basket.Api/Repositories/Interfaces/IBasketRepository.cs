using Basket.Api.Entities;
using Microsoft.Extensions.Caching.Distributed;

namespace Repositories.Interfaces;

public interface IBasketRepository
{
    Task<Cart?> GetByUserName(string username);
    Task<Cart> UpdateBasket(Cart cart, DistributedCacheEntryOptions options = null);
    Task<bool> DeleteBasketFromUserName(string username);
}