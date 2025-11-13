using ApiExpanda.Application.DTOs;

namespace ApiExpanda.Application.Services.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
    Task<IEnumerable<CategoryDto>> GetCategoriesOrderedByIdAsync();
    Task<CategoryDto?> GetCategoryByIdAsync(int id);
    Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto);
    Task<bool> UpdateCategoryAsync(int id, CreateCategoryDto updateCategoryDto);
    Task<bool> DeleteCategoryAsync(int id);
    Task<bool> CategoryExistsAsync(int id);
    Task<bool> CategoryExistsByNameAsync(string name);
}
