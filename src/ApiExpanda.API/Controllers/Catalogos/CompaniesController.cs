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
public class CompaniesController : ControllerBase
{
    private readonly ICompanyService _companyService;

    public CompaniesController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    /// <summary>
    /// Obtiene todas las compañías
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetCompanies()
    {
        var companies = await _companyService.GetAllCompaniesAsync();
        return Ok(companies);
    }

    /// <summary>
    /// Obtiene una compañía por ID
    /// </summary>
    [HttpGet("{id:int}", Name = "GetCompany")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCompany(int id)
    {
        var company = await _companyService.GetCompanyByIdAsync(id);
        if (company == null)
        {
            return NotFound($"La compañía con ID {id} no existe.");
        }
        return Ok(company);
    }

    /// <summary>
    /// Obtiene una compañía por código
    /// </summary>
    [HttpGet("by-code/{code}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCompanyByCode(string code)
    {
        var company = await _companyService.GetCompanyByCodeAsync(code);
        if (company == null)
        {
            return NotFound($"La compañía con código '{code}' no existe.");
        }
        return Ok(company);
    }

    /// <summary>
    /// Crea una nueva compañía
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyDto createCompanyDto)
    {
        if (createCompanyDto == null)
        {
            return BadRequest("Los datos de la compañía son requeridos.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var company = await _companyService.CreateCompanyAsync(createCompanyDto);
            return CreatedAtRoute("GetCompany", new { id = company.Id }, company);
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return BadRequest(ModelState);
        }
    }

    /// <summary>
    /// Actualiza una compañía existente
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateCompany(int id, [FromBody] UpdateCompanyDto updateCompanyDto)
    {
        if (updateCompanyDto == null)
        {
            return BadRequest("Los datos de la compañía son requeridos.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = await _companyService.UpdateCompanyAsync(id, updateCompanyDto);
            if (!result)
            {
                return StatusCode(500, "Error al actualizar la compañía.");
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
    public async Task<IActionResult> DeleteCompany(int id)
    {
        if (!await _companyService.CompanyExistsAsync(id))
        {
            return NotFound($"La compañía con ID {id} no existe.");
        }

        try
        {
            var result = await _companyService.DeleteCompanyAsync(id);
            if (!result)
            {
                return StatusCode(500, "Error al eliminar la compañía.");
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
    /// Verifica si una compañía existe por ID
    /// </summary>
    [HttpGet("exists/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CompanyExists(int id)
    {
        var exists = await _companyService.CompanyExistsAsync(id);
        return Ok(new { exists });
    }

    /// <summary>
    /// Verifica si una compañía existe por código
    /// </summary>
    [HttpGet("exists/by-code/{code}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CompanyExistsByCode(string code)
    {
        var exists = await _companyService.CompanyExistsByCodeAsync(code);
        return Ok(new { exists });
    }
}
