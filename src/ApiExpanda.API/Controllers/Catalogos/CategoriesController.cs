using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApiExpanda.Application.Modules.Catalogos.DTOs;
using ApiExpanda.Application.Modules.Catalogos.Services.Interfaces;
using ApiExpanda.Constants;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;

namespace ApiExpanda.API.Controllers.Catalogos;

[ApiController]
[Route("api/v{version:apiVersion}/Catalogos/[controller]")]
[ApiVersion("1.0")]
[Authorize(Roles = "Admin")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetCategories()
    {
        var categoriesDto = await _categoryService.GetAllCategoriesAsync();
        return Ok(categoriesDto);
    }

    [AllowAnonymous]
    [HttpGet("ordered")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetCategoriesOrderedById()
    {
        var categoriesDto = await _categoryService.GetCategoriesOrderedByIdAsync();
        return Ok(categoriesDto);
    }

    [AllowAnonymous]
    [HttpGet("{id:int}", Name = "GetCategoryById")]
    [ResponseCache(CacheProfileName = CacheProfiles.Default20)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetCategory(int id)
    {
        var categoryDto = await _categoryService.GetCategoryByIdAsync(id);
        if (categoryDto == null)
        {
            return NotFound("La categoría con el id especificado no existe.");
        }

        return Ok(categoryDto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (await _categoryService.CategoryExistsByNameAsync(createCategoryDto.Name))
        {
            ModelState.AddModelError("CustomError", "La categoría ya existe.");
            return BadRequest(ModelState);
        }

        try
        {
            var categoryDto = await _categoryService.CreateCategoryAsync(createCategoryDto);
            return CreatedAtRoute("GetCategoryById", new { id = categoryDto.Id }, categoryDto);
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("CustomError", ex.Message);
            return StatusCode(500, ModelState);
        }
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] CreateCategoryDto updateCategoryDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (!await _categoryService.CategoryExistsAsync(id))
        {
            return NotFound("La categoría con el id especificado no existe.");
        }

        try
        {
            var result = await _categoryService.UpdateCategoryAsync(id, updateCategoryDto);
            if (!result)
            {
                ModelState.AddModelError("CustomError", "Algo salió mal actualizando el registro");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("CustomError", ex.Message);
            return StatusCode(500, ModelState);
        }
    }
    
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        if (!await _categoryService.CategoryExistsAsync(id))
        {
            return NotFound("La categoría con el id especificado no existe.");
        }

        try
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            if (!result)
            {
                ModelState.AddModelError("CustomError", "Algo salió mal eliminando el registro");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("CustomError", ex.Message);
            return StatusCode(500, ModelState);
        }
    }

    [AllowAnonymous]
    [HttpGet("exists/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CategoryExists(int id)
    {
        var exists = await _categoryService.CategoryExistsAsync(id);
        return Ok(new { exists });
    }

    [AllowAnonymous]
    [HttpGet("exists/by-name/{name}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CategoryExistsByName(string name)
    {
        var exists = await _categoryService.CategoryExistsByNameAsync(name);
        return Ok(new { exists });
    }
}
