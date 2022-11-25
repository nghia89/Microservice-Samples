using AutoMapper;
using Basket.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;
using Shared.DTOs.Basket;

namespace Basket.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class BasketController : ControllerBase
{
    private readonly ILogger<BasketController> _logger;
    private readonly IMapper _mapper;
    private readonly IBasketRepository _basketRepository;
    public BasketController(ILogger<BasketController> logger, IBasketRepository basketRepository, IMapper mapper)
    {
        _logger = logger;
        _basketRepository = basketRepository;
        _mapper = mapper;
    }

    [HttpPost(Name = "update")]
    public async Task<ActionResult<Cart>> Update(CartDto cart)
    {
        var model = _mapper.Map<Cart>(cart);
        var data = await _basketRepository.UpdateBasket(model);
        return Ok(data);
    }

    [HttpGet(Name = "get/{username}")]
    public async Task<ActionResult<Cart>> Get(string username)
    {
        var data = await _basketRepository.GetByUserName(username);
        return Ok(data);
    }
}
