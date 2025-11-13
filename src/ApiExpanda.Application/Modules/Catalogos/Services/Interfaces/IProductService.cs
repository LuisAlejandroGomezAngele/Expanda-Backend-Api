using ApiExpanda.Application.Modules.Catalogos.DTOs;
using ApiExpanda.Application.DTOs.Responses;

namespace ApiExpanda.Application.Modules.Catalogos.Services.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllProductsAsync();
    Task<ProductDto?> GetProductByIdAsync(int productId);
    Task<PaginationResponse<ProductDto>> GetProductsInPageAsync(int page, int pageSize);
    Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto, string? imagePath = null);
    Task<bool> UpdateProductAsync(int productId, UpdateProductDto updateProductDto, string? imagePath = null);
    Task<bool> DeleteProductAsync(int productId);
    Task<IEnumerable<ProductDto>> GetProductsForCategoryAsync(int categoryId);
    Task<IEnumerable<ProductDto>> SearchProductAsync(string name);
    Task<bool> BuyProductAsync(string name, int quantity);
    Task<bool> ProductExistsAsync(int productId);
    Task<bool> ProductExistsByNameAsync(string name);
}
