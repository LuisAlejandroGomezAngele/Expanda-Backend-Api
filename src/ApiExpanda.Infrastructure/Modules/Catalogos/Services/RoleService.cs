using ApiExpanda.Application.Modules.Catalogos.DTOs;
using ApiExpanda.Application.Modules.Catalogos.Interfaces;
using ApiExpanda.Application.Modules.Catalogos.Services.Interfaces;
using ApiExpanda.Domain.Modules.Catalogos.Entities;
using MapsterMapper;

namespace ApiExpanda.Infrastructure.Modules.Catalogos.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;

    public RoleService(IRoleRepository roleRepository, IMapper mapper)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
    }
    public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
    {
        var roles = await Task.Run(() => _roleRepository.GetRoles());
        return _mapper.Map<IEnumerable<RoleDto>>(roles);
    }
    public async Task<RoleDto?> GetRoleByIdAsync(int id)
    {
        var role = await Task.Run(() => _roleRepository.GetRole(id));
        if (role == null)
            return null;

        return _mapper.Map<RoleDto>(role);
    }
    public async Task<RoleDto> CreateRoleAsync(CreateRoleDto createRoleDto)
    {
        var role = _mapper.Map<Roles>(createRoleDto);

        var created = await Task.Run(() => _roleRepository.CreateRole(role));
        if (!created)
            throw new InvalidOperationException("Error al crear el rol");

        return _mapper.Map<RoleDto>(role);
    }
    public async Task<bool> UpdateRoleAsync(int id, UpdateRoleDto updateRoleDto)
    {
        // Verificar que el rol existe usando el método directo del repositorio
        if (!await Task.Run(() => _roleRepository.RoleExists(id)))
        {
            throw new InvalidOperationException("El rol no existe");
        }

        var role = _mapper.Map<Roles>(updateRoleDto);
        role.Id = id;

        return await Task.Run(() => _roleRepository.UpdateRole(role));
    }


    public async Task<bool> DeleteRoleAsync(int id)
    {
        // Verificar que el rol existe
        var role = await Task.Run(() => _roleRepository.GetRole(id));
        if (role == null)
        {
            throw new InvalidOperationException("El rol no existe");
        }

        return await Task.Run(() => _roleRepository.DeleteRole(role));
    }

    public async Task<bool> RoleExistsAsync(int id)
    {
        var role = await Task.Run(() => _roleRepository.GetRole(id));
        return role != null;
    }
}
