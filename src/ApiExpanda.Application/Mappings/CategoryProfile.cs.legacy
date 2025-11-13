using ApiExpanda.Domain.Entities;
using ApiExpanda.Application.DTOs;
using Mapster;

namespace ApiExpanda.Application.Mappings;

public static class CategoryMapping
{
    public static void RegisterMappings(TypeAdapterConfig config)
    {
        config.NewConfig<Category, CategoryDto>().TwoWays();
        config.NewConfig<Category, CreateCategoryDto>().TwoWays();
        config.NewConfig<Category, UpdateCategoryDto>().TwoWays();
    }
}
