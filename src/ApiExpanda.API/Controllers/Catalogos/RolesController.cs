using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ApiExpanda.Application.Modules.Catalogos.DTOs;
using ApiExpanda.Application.Modules.Catalogos.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;

namespace ApiExpanda.API.Controllers.Catalogos;

[ApiController]
[Route("api/v{version:apiVersion}/Catalogos/[controller]")]
[ApiVersion("1.0")]
[Authorize]

public class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    /// <summary>
    /// Obtiene todos los roles
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetRoles()
    {
        var roles = await _roleService.GetAllRolesAsync();
        return Ok(roles);
    }

    /// <summary>
    /// Obtiene un rol por ID
    /// </summary>
    [HttpGet("{id:int}", Name = "GetRole")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetRole(int id)
    {
        var role = await _roleService.GetRoleByIdAsync(id);
        if (role == null)
        {
            return NotFound($"El rol con ID {id} no existe.");
        }
        return Ok(role);
    }

    /// <summary>
    /// Crea una nueva compañía
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto createRoleDto)
    {
        if (createRoleDto == null)
        {
            return BadRequest("Los datos del rol son requeridos.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var role = await _roleService.CreateRoleAsync(createRoleDto);
            return CreatedAtRoute("GetRole", new { id = role.Id }, role);
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return BadRequest(ModelState);
        }
    }

    /// <summary>
    /// Actualiza un rol existente
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateRole(int id, [FromBody] UpdateRoleDto updateRoleDto)
    {
        if (updateRoleDto == null)
        {
            return BadRequest("Los datos del rol son requeridos.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = await _roleService.UpdateRoleAsync(id, updateRoleDto);
            if (!result)
            {
                return StatusCode(500, "Error al actualizar el rol.");
            }
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return BadRequest(ModelState);
        }
    }

    /// <summary>
    /// Elimina una compañía
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteRole(int id)
    {
        if (!await _roleService.RoleExistsAsync(id))
        {
            return NotFound($"El rol con ID {id} no existe.");
        }

        try
        {
            var result = await _roleService.DeleteRoleAsync(id);
            if (!result)
            {
                return StatusCode(500, "Error al eliminar el rol.");
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return StatusCode(500, ModelState);
        }
    }

    /// <summary>
    /// Verifica si un rol existe por ID
    /// </summary>
    [HttpGet("exists/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RoleExists(int id)
    {
        var exists = await _roleService.RoleExistsAsync(id);
        return Ok(new { exists });
    }
}
