using ApiExpanda.Application.Modules.Catalogos.Interfaces;
using ApiExpanda.Domain.Modules.Catalogos.Entities;
using ApiExpanda.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiExpanda.Infrastructure.Modules.Catalogos.Repositories;

public class RolesRepository : IRoleRepository
{
    private readonly ApplicationDbContext _context;

    public RolesRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public ICollection<Roles> GetRoles()
    {
        return _context.Set<Roles>()
            .OrderBy(c => c.Name)
            .ToList();
    }

    public Roles? GetRole(int id)
    {
        return _context.Set<Roles>()
            .FirstOrDefault(c => c.Id == id);
    }

    public bool CreateRole(Roles role)
    {
        role.CreatedAt = DateTime.Now;
        _context.Set<Roles>().Add(role);
        return Save();
    }

    public bool UpdateRole(Roles role)
    {
        // Desadjuntar cualquier instancia existente que esté siendo rastreada
        var existingEntity = _context.Set<Roles>().Local.FirstOrDefault(r => r.Id == role.Id);
        if (existingEntity != null)
        {
            _context.Entry(existingEntity).State = EntityState.Detached;
        }

        role.UpdatedAt = DateTime.Now;
        _context.Set<Roles>().Update(role);
        return Save();
    }

    public bool DeleteRole(Roles role)
    {
        _context.Set<Roles>().Remove(role);
        return Save();
    }

    public bool RoleExists(int id)
    {
        return _context.Set<Roles>().Any(c => c.Id == id);
    } 

    public bool Save()
    {
        return _context.SaveChanges() >= 0;
    }
}
