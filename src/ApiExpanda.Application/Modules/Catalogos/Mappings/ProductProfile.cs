using ApiExpanda.Domain.Modules.Catalogos.Entities;
using ApiExpanda.Application.Modules.Catalogos.DTOs;
using Mapster;

namespace ApiExpanda.Application.Modules.Catalogos.Mappings;

public static class ProductMapping
{
    public static void RegisterMappings(TypeAdapterConfig config)
    {
        config.NewConfig<Product, ProductDto>()
            .Map(dest => dest.CategoryName, src => src.Category != null ? src.Category.Name : string.Empty);
        
        config.NewConfig<CreateProductDto, Product>();
        config.NewConfig<UpdateProductDto, Product>();
    }
}
