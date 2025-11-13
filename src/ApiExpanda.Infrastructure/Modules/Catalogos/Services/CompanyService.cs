using ApiExpanda.Application.Modules.Catalogos.DTOs;
using ApiExpanda.Application.Modules.Catalogos.Interfaces;
using ApiExpanda.Application.Modules.Catalogos.Services.Interfaces;
using ApiExpanda.Domain.Modules.Catalogos.Entities;
using MapsterMapper;

namespace ApiExpanda.Infrastructure.Modules.Catalogos.Services;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IMapper _mapper;

    public CompanyService(ICompanyRepository companyRepository, IMapper mapper)
    {
        _companyRepository = companyRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync()
    {
        var companies = await Task.Run(() => _companyRepository.GetCompanies());
        return _mapper.Map<IEnumerable<CompanyDto>>(companies);
    }

    public async Task<CompanyDto?> GetCompanyByIdAsync(int id)
    {
        var company = await Task.Run(() => _companyRepository.GetCompany(id));
        if (company == null)
            return null;

        return _mapper.Map<CompanyDto>(company);
    }

    public async Task<CompanyDto?> GetCompanyByCodeAsync(string code)
    {
        var company = await Task.Run(() => _companyRepository.GetCompanyByCode(code));
        if (company == null)
            return null;

        return _mapper.Map<CompanyDto>(company);
    }

    public async Task<CompanyDto> CreateCompanyAsync(CreateCompanyDto createCompanyDto)
    {
        // Validar que el código no exista
        if (await CompanyExistsByCodeAsync(createCompanyDto.Code))
        {
            throw new InvalidOperationException($"Ya existe una compañía con el código '{createCompanyDto.Code}'");
        }

        var company = _mapper.Map<Company>(createCompanyDto);
        
        var created = await Task.Run(() => _companyRepository.CreateCompany(company));
        if (!created)
            throw new InvalidOperationException("Error al crear la compañía");

        return _mapper.Map<CompanyDto>(company);
    }

    public async Task<bool> UpdateCompanyAsync(int id, UpdateCompanyDto updateCompanyDto)
    {
        // Verificar que la compañía existe
        if (!await CompanyExistsAsync(id))
        {
            throw new InvalidOperationException("La compañía no existe");
        }

        // Validar que el código no esté en uso por otra compañía
        var existingCompany = await Task.Run(() => _companyRepository.GetCompanyByCode(updateCompanyDto.Code));
        if (existingCompany != null && existingCompany.Id != id)
        {
            throw new InvalidOperationException($"Ya existe otra compañía con el código '{updateCompanyDto.Code}'");
        }

        var company = _mapper.Map<Company>(updateCompanyDto);
        company.Id = id;

        return await Task.Run(() => _companyRepository.UpdateCompany(company));
    }

    public async Task<bool> DeleteCompanyAsync(int id)
    {
        var company = await Task.Run(() => _companyRepository.GetCompany(id));
        if (company == null)
            return false;

        return await Task.Run(() => _companyRepository.DeleteCompany(company));
    }

    public async Task<bool> CompanyExistsAsync(int id)
    {
        return await Task.Run(() => _companyRepository.CompanyExists(id));
    }

    public async Task<bool> CompanyExistsByCodeAsync(string code)
    {
        return await Task.Run(() => _companyRepository.CompanyExistsByCode(code));
    }
}
