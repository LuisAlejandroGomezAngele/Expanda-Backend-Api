using ApiExpanda.Application.Modules.Catalogos.DTOs;
using ApiExpanda.Domain.Modules.Catalogos.Entities;
using Mapster;

namespace ApiExpanda.Application.Modules.Catalogos.Mappings;

public static class RoleMapping
{
    public static void RegisterMappings(TypeAdapterConfig config)
    {
        config.NewConfig<Roles, RoleDto>();

        config.NewConfig<CreateRoleDto, Roles>()
            .Map(dest => dest.CreatedAt, src => DateTime.Now)
            .Map(dest => dest.UpdatedAt, src => (DateTime?)null);

        config.NewConfig<UpdateRoleDto, Roles>()
            .Map(dest => dest.UpdatedAt, src => DateTime.Now);
    }
}
