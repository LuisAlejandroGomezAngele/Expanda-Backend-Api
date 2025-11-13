using ApiExpanda.Application.Modules.Catalogos.DTOs;

namespace ApiExpanda.Application.Modules.Catalogos.Services.Interfaces;

public interface ICompanyService
{
    Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync();
    Task<CompanyDto?> GetCompanyByIdAsync(int id);
    Task<CompanyDto?> GetCompanyByCodeAsync(string code);
    Task<CompanyDto> CreateCompanyAsync(CreateCompanyDto createCompanyDto);
    Task<bool> UpdateCompanyAsync(int id, UpdateCompanyDto updateCompanyDto);
    Task<bool> DeleteCompanyAsync(int id);
    Task<bool> CompanyExistsAsync(int id);
    Task<bool> CompanyExistsByCodeAsync(string code);
}
