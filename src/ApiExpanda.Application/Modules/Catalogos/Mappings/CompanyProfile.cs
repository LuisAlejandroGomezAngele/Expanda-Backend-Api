using ApiExpanda.Application.Modules.Catalogos.DTOs;
using ApiExpanda.Domain.Modules.Catalogos.Entities;
using Mapster;

namespace ApiExpanda.Application.Modules.Catalogos.Mappings;

public static class CompanyMapping
{
    public static void RegisterMappings(TypeAdapterConfig config)
    {
        config.NewConfig<Company, CompanyDto>();
        
        config.NewConfig<CreateCompanyDto, Company>()
            .Map(dest => dest.CreateAt, src => DateTime.Now)
            .Map(dest => dest.UpdateAt, src => (DateTime?)null);
        
        config.NewConfig<UpdateCompanyDto, Company>()
            .Map(dest => dest.UpdateAt, src => DateTime.Now);
    }
}
