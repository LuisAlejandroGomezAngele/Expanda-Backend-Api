using ApiExpanda.Domain.Modules.Catalogos.Entities;
using ApiExpanda.Application.Modules.Catalogos.DTOs;
using Mapster;

namespace ApiExpanda.Application.Modules.Catalogos.Mappings;

public static class CategoryMapping
{
    public static void RegisterMappings(TypeAdapterConfig config)
    {
        config.NewConfig<Category, CategoryDto>().TwoWays();
        config.NewConfig<Category, CreateCategoryDto>().TwoWays();
        config.NewConfig<Category, UpdateCategoryDto>().TwoWays();
    }
}
