using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ApiExpanda.Application.DTOs;
using ApiExpanda.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;


namespace ApiExpanda.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")] //http://localhost:5000/api/products
[ApiVersionNeutral]

public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProducts()
    {
        var productsDto = await _productService.GetAllProductsAsync();
        return Ok(productsDto);
    }

    [AllowAnonymous]
    [HttpGet("{productId:int}", Name = "GetProduct")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
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
    [HttpGet("Paged", Name = "GetProductsInPage")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductsInPage([FromQuery] int page = 1, int pageSize = 10)
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
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateProduct([FromForm] CreateProductDto createProductDto)
    {
        if (createProductDto == null)
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
            return CreatedAtRoute("GetProduct", new { productId = productDto.ProductId }, productDto);
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

        var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}/{HttpContext.Request.PathBase.Value}";
        return $"{baseUrl}ProductsImages/{fileName}";
    }

    [HttpGet("searchProductByCategory/{categoryId:int}", Name = "GetProductsForCategory")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductsForCategory(int categoryId)
    {
        var productsDto = await _productService.GetProductsForCategoryAsync(categoryId);
        if (productsDto == null || !productsDto.Any())
        {
            return NotFound("No se encontraron productos para la categoría especificada.");
        }
        return Ok(productsDto);
    }

    [HttpGet("searchProductByNameDescription/{name}", Name = "GetProductsForNameDescription")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductsForNameDescription(string name)
    {
        var productsDto = await _productService.SearchProductAsync(name);
        if (productsDto == null || !productsDto.Any())
        {
            return NotFound("No se encontraron productos para el nombre o descripción especificada.");
        }
        return Ok(productsDto);
    }

    [HttpPatch("buyProduct/{name}/{quantity:int}", Name = "BuyProduct")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
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

    [HttpPut("{productId:int}", Name = "UpdateProduct")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateProduct(int productId, [FromForm] UpdateProductDto updateProductDto)
    {
        if (updateProductDto == null)
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

    [HttpDelete("{productId:int}", Name = "DeleteProduct")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
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

}
