namespace Product.Api;

using AutoMapper;
using Product.API.Entities;
using Shared.DTOs.Product;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CatalogProduct, ProductDto>().ReverseMap();
        CreateMap<CatalogProduct, CreateProductDto>().ReverseMap();
        CreateMap<CatalogProduct, UpdateProductDto>().ReverseMap();

    }


}