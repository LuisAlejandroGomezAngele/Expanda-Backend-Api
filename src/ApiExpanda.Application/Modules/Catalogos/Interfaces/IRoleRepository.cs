using ApiExpanda.Domain.Modules.Catalogos.Entities;

namespace ApiExpanda.Application.Modules.Catalogos.Interfaces;

public interface IRoleRepository
{
    ICollection<Roles> GetRoles();
    Roles? GetRole(int id);
    bool CreateRole(Roles role);
    bool UpdateRole(Roles role);
    bool DeleteRole(Roles role);
    bool RoleExists(int id);
    bool Save();
}
