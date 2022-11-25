using Basket.Api.Entities;
using Infrastructure.Common.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Repositories.Interfaces;


namespace Repositories;

public class BasketRepository : IBasketRepository
{
    private readonly IDistributedCache _cache;
    private readonly ISerializeService _serializeService;
    public BasketRepository(IDistributedCache cache, ISerializeService serializeService)
    {
        _cache = cache;
        _serializeService = serializeService;
    }

    public async Task<bool> DeleteBasketFromUserName(string username)
    {
        try
        {
            await _cache.RemoveAsync(username);
            return true;
        }
        catch (System.Exception)
        {
            return false;
            throw;
        }
    }

    public async Task<Cart?> GetByUserName(string username)
    {
        var data = await _cache.GetStringAsync(username);
        return _serializeService.Deserialize<Cart>(data);
    }

    public async Task<Cart> UpdateBasket(Cart cart, DistributedCacheEntryOptions options = null)
    {
        if (options != null)
            await _cache.SetStringAsync(cart.UserName,
                _serializeService.Serialize(cart), options);
        else
            await _cache.SetStringAsync(cart.UserName,
                _serializeService.Serialize(cart));

        var data = await GetByUserName(cart.UserName);
        return data ?? new Cart();
    }
}