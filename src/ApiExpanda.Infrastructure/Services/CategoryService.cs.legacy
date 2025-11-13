using ApiExpanda.Application.DTOs;
using ApiExpanda.Application.Interfaces;
using ApiExpanda.Application.Services.Interfaces;
using ApiExpanda.Domain.Entities;
using MapsterMapper;

namespace ApiExpanda.Infrastructure.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
    {
        var categories = await Task.Run(() => _categoryRepository.GetCategories());
        return _mapper.Map<IEnumerable<CategoryDto>>(categories);
    }

    public async Task<IEnumerable<CategoryDto>> GetCategoriesOrderedByIdAsync()
    {
        var categories = await Task.Run(() => _categoryRepository.GetCategories().OrderBy(c => c.Id));
        return _mapper.Map<IEnumerable<CategoryDto>>(categories);
    }

    public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
    {
        var category = await Task.Run(() => _categoryRepository.GetCategory(id));
        if (category == null)
            return null;

        return _mapper.Map<CategoryDto>(category);
    }

    public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto)
    {
        var category = _mapper.Map<Category>(createCategoryDto);
        
        var created = await Task.Run(() => _categoryRepository.CreateCategory(category));
        if (!created)
            throw new InvalidOperationException("Error al crear la categoría");

        return _mapper.Map<CategoryDto>(category);
    }

    public async Task<bool> UpdateCategoryAsync(int id, CreateCategoryDto updateCategoryDto)
    {
        var category = _mapper.Map<Category>(updateCategoryDto);
        category.Id = id;

        return await Task.Run(() => _categoryRepository.UpdateCategory(category));
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        var category = await Task.Run(() => _categoryRepository.GetCategory(id));
        if (category == null)
            return false;

        return await Task.Run(() => _categoryRepository.DeleteCategory(category));
    }

    public async Task<bool> CategoryExistsAsync(int id)
    {
        return await Task.Run(() => _categoryRepository.CategoryExists(id));
    }

    public async Task<bool> CategoryExistsByNameAsync(string name)
    {
        return await Task.Run(() => _categoryRepository.CategoryExists(name));
    }
}
