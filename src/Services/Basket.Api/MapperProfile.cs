using AutoMapper;
using Basket.Api.Entities;
using Shared.DTOs.Basket;

namespace Basket.Api;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CartDto, Cart>().ReverseMap();
        CreateMap<CartItemDto, CartItem>().ReverseMap();
    }


}