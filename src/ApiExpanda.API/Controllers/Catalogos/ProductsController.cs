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
[Authorize(Roles = "Admin")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetProducts()
    {
        var productsDto = await _productService.GetAllProductsAsync();
        return Ok(productsDto);
    }

    [AllowAnonymous]
    [HttpGet("{productId:int}", Name = "GetProductById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProduct(int productId)
    {
        var productDto = await _productService.GetProductByIdAsync(productId);
        if (productDto == null)
        {
            return NotFound("El producto con el id especificado no existe.");
        }
        return Ok(productDto);
    }

    [AllowAnonymous]
    [HttpGet("paged")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductsInPage([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page <= 0 || pageSize <= 0)
        {
            return BadRequest("El número de página y el tamaño de página deben ser mayores que cero.");
        }

        var response = await _productService.GetProductsInPageAsync(page, pageSize);
        
        if (response.Items == null || !response.Items.Any())
        {
            return NotFound("No se encontraron productos.");
        }

        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateProduct([FromForm] CreateProductDto createProductDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            string? imagePath = null;
            
            if (createProductDto.Image != null)
            {
                imagePath = await SaveProductImageAsync(createProductDto.Image);
            }

            var productDto = await _productService.CreateProductAsync(createProductDto, imagePath);
            return CreatedAtRoute("GetProductById", new { productId = productDto.ProductId }, productDto);
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("CustomError", ex.Message);
            return BadRequest(ModelState);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("CustomError", $"Error: {ex.Message}");
            return StatusCode(500, ModelState);
        }
    }

    [HttpPut("{productId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateProduct(int productId, [FromForm] UpdateProductDto updateProductDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            string? imagePath = null;
            
            if (updateProductDto.Image != null)
            {
                imagePath = await SaveProductImageAsync(updateProductDto.Image);
            }

            var result = await _productService.UpdateProductAsync(productId, updateProductDto, imagePath);
            if (!result)
            {
                ModelState.AddModelError("CustomError", "Algo salió mal actualizando el producto");
                return StatusCode(500, ModelState);
            }
            
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("CustomError", ex.Message);
            return BadRequest(ModelState);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("CustomError", $"Error: {ex.Message}");
            return StatusCode(500, ModelState);
        }
    }

    [HttpDelete("{productId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProduct(int productId)
    {
        if (productId <= 0)
        {
            return BadRequest(ModelState);
        }

        var product = await _productService.GetProductByIdAsync(productId);
        if (product == null)
        {
            return NotFound("El producto con el id especificado no existe.");
        }

        try
        {
            var result = await _productService.DeleteProductAsync(productId);
            if (!result)
            {
                ModelState.AddModelError("CustomError", "Algo salió mal eliminando el producto");
                return StatusCode(500, ModelState);
            }
            
            return NoContent();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("CustomError", $"Error: {ex.Message}");
            return StatusCode(500, ModelState);
        }
    }

    [AllowAnonymous]
    [HttpGet("by-category/{categoryId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductsForCategory(int categoryId)
    {
        var productsDto = await _productService.GetProductsForCategoryAsync(categoryId);
        if (productsDto == null || !productsDto.Any())
        {
            return NotFound("No se encontraron productos para la categoría especificada.");
        }
        return Ok(productsDto);
    }

    [AllowAnonymous]
    [HttpGet("search/{name}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SearchProducts(string name)
    {
        var productsDto = await _productService.SearchProductAsync(name);
        if (productsDto == null || !productsDto.Any())
        {
            return NotFound("No se encontraron productos para el nombre o descripción especificada.");
        }
        return Ok(productsDto);
    }

    [HttpPatch("buy/{name}/{quantity:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> BuyProduct(string name, int quantity)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest("El nombre del producto no puede estar vacío.");
        }
        if (quantity <= 0)
        {
            return BadRequest("La cantidad debe ser mayor que cero.");
        }

        try
        {
            await _productService.BuyProductAsync(name, quantity);
            var units = quantity == 1 ? "unidad" : "unidades";
            return Ok($"Compra exitosa de {quantity} {units} del producto '{name}'.");
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("CustomError", ex.Message);
            return BadRequest(ModelState);
        }
    }

    [AllowAnonymous]
    [HttpGet("exists/{productId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ProductExists(int productId)
    {
        var exists = await _productService.ProductExistsAsync(productId);
        return Ok(new { exists });
    }

    [AllowAnonymous]
    [HttpGet("exists/by-name/{name}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ProductExistsByName(string name)
    {
        var exists = await _productService.ProductExistsByNameAsync(name);
        return Ok(new { exists });
    }

    private async Task<string> SaveProductImageAsync(IFormFile image)
    {
        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
        var imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProductsImages");
        
        if (!Directory.Exists(imageFolder))
        {
            Directory.CreateDirectory(imageFolder);
        }

        var filePath = Path.Combine(imageFolder, fileName);
        FileInfo file = new FileInfo(filePath);
        
        if (file.Exists)
        {
            file.Delete();
        }
        
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await image.CopyToAsync(stream);
        }

        var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
        baseUrl = baseUrl.TrimEnd('/');
        return $"{baseUrl}/ProductsImages/{fileName}";
    }
}
