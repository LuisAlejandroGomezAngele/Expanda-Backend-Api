using ApiExpanda.Domain.Modules.Catalogos.Entities;

namespace ApiExpanda.Application.Modules.Catalogos.Interfaces;

public interface ICompanyRepository
{
    ICollection<Company> GetCompanies();
    Company? GetCompany(int id);
    Company? GetCompanyByCode(string code);
    bool CreateCompany(Company company);
    bool UpdateCompany(Company company);
    bool DeleteCompany(Company company);
    bool CompanyExists(int id);
    bool CompanyExistsByCode(string code);
    bool Save();
}
