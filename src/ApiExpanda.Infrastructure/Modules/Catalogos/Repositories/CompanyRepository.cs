using ApiExpanda.Application.Modules.Catalogos.Interfaces;
using ApiExpanda.Domain.Modules.Catalogos.Entities;
using ApiExpanda.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiExpanda.Infrastructure.Modules.Catalogos.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly ApplicationDbContext _context;

    public CompanyRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public ICollection<Company> GetCompanies()
    {
        return _context.Set<Company>()
            .OrderBy(c => c.Name)
            .ToList();
    }

    public Company? GetCompany(int id)
    {
        return _context.Set<Company>()
            .FirstOrDefault(c => c.Id == id);
    }

    public Company? GetCompanyByCode(string code)
    {
        return _context.Set<Company>()
            .FirstOrDefault(c => c.Code == code);
    }

    public bool CreateCompany(Company company)
    {
        company.CreateAt = DateTime.Now;
        _context.Set<Company>().Add(company);
        return Save();
    }

    public bool UpdateCompany(Company company)
    {
        company.UpdateAt = DateTime.Now;
        _context.Set<Company>().Update(company);
        return Save();
    }

    public bool DeleteCompany(Company company)
    {
        _context.Set<Company>().Remove(company);
        return Save();
    }

    public bool CompanyExists(int id)
    {
        return _context.Set<Company>().Any(c => c.Id == id);
    }

    public bool CompanyExistsByCode(string code)
    {
        return _context.Set<Company>().Any(c => c.Code == code);
    }

    public bool Save()
    {
        return _context.SaveChanges() >= 0;
    }
}
