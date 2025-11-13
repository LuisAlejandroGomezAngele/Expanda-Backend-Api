using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApiExpanda.Application.DTOs;
using ApiExpanda.Application.Services.Interfaces;
using ApiExpanda.Constants;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;

namespace ApiExpanda.Controllers.V2;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")] //http://localhost:5000/api/categories
[ApiVersion("2.0")]
[Authorize(Roles = "Admin")]

//[EnableCors(PolicyNames.AllowSpecificOrigin)]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategoriesOrderById()
    {
        var categoriesDto = await _categoryService.GetCategoriesOrderedByIdAsync();
        return Ok(categoriesDto);
    }


    [AllowAnonymous]
    [HttpGet("{id:int}", Name = "GetCategory")]
    [ResponseCache(CacheProfileName = CacheProfiles.Default20)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCategory(int id)
    {
        var categoryDto = await _categoryService.GetCategoryByIdAsync(id);
        if (categoryDto == null)
        {
            return NotFound("La categoria con el id especificado no existe.");
        }

        return Ok(categoryDto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
    {
        if (createCategoryDto == null)
        {
            return BadRequest(ModelState);
        }

        if (await _categoryService.CategoryExistsByNameAsync(createCategoryDto.Name))
        {
            ModelState.AddModelError("CustomError", "La categoria ya existe.");
            return BadRequest(ModelState);
        }

        try
        {
            var categoryDto = await _categoryService.CreateCategoryAsync(createCategoryDto);
            return CreatedAtRoute("GetCategory", new { id = categoryDto.Id }, categoryDto);
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("CustomError", ex.Message);
            return StatusCode(500, ModelState);
        }
    }

    [HttpPatch("{id:int}", Name = "UpdateCategory")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] CreateCategoryDto updateCategoryDto)
    {
        if (updateCategoryDto == null)
        {
            return BadRequest(ModelState);
        }

        if (!await _categoryService.CategoryExistsAsync(id))
        {
            return NotFound("La categoria con el id especificado no existe.");
        }

        if (await _categoryService.CategoryExistsByNameAsync(updateCategoryDto.Name))
        {
            ModelState.AddModelError("CustomError", "La categoria ya existe.");
            return BadRequest(ModelState);
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
    
    [HttpDelete("{id:int}", Name = "DeleteCategory")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        if (!await _categoryService.CategoryExistsAsync(id))
        {
            return NotFound("La categoria con el id especificado no existe.");
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
}
