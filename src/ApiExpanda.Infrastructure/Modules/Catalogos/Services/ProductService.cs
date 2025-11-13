using ApiExpanda.Application.Modules.Catalogos.DTOs;
using ApiExpanda.Application.DTOs.Responses;
using ApiExpanda.Application.Modules.Catalogos.Interfaces;
using ApiExpanda.Application.Modules.Catalogos.Services.Interfaces;
using ApiExpanda.Domain.Modules.Catalogos.Entities;
using MapsterMapper;

namespace ApiExpanda.Infrastructure.Modules.Catalogos.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public ProductService(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        var products = await Task.Run(() => _productRepository.GetProducts());
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<ProductDto?> GetProductByIdAsync(int productId)
    {
        var product = await Task.Run(() => _productRepository.GetProduct(productId));
        if (product == null)
            return null;

        return _mapper.Map<ProductDto>(product);
    }

    public async Task<PaginationResponse<ProductDto>> GetProductsInPageAsync(int page, int pageSize)
    {
        var totalProducts = await Task.Run(() => _productRepository.GetTotalProducts());
        var totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);
        var products = await Task.Run(() => _productRepository.GetProductsInPages(page, pageSize));

        var productsDto = _mapper.Map<List<ProductDto>>(products);

        return new PaginationResponse<ProductDto>
        {
            TotalItems = totalProducts,
            PageSize = pageSize,
            PageNumber = page,
            TotalPages = totalPages,
            Items = productsDto
        };
    }

    public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto, string? imagePath = null)
    {
        // Validar que la categoría existe
        if (!_categoryRepository.CategoryExists(createProductDto.CategoryId))
        {
            throw new InvalidOperationException("La categoría especificada no existe.");
        }

        // Validar que el producto no existe
        if (_productRepository.ProductExists(createProductDto.Name))
        {
            throw new InvalidOperationException("El producto ya existe.");
        }

        var product = _mapper.Map<Product>(createProductDto);

        // Asignar imagen si fue proporcionada
        if (!string.IsNullOrEmpty(imagePath))
        {
            product.ImgUrl = imagePath;
        }
        else if (!string.IsNullOrEmpty(createProductDto.ImgUrl))
        {
            product.ImgUrl = createProductDto.ImgUrl;
        }
        else
        {
            product.ImgUrl = "https://placehold.com/600x400";
        }

        var created = await Task.Run(() => _productRepository.CreateProduct(product));
        if (!created)
            throw new InvalidOperationException($"Error al crear el producto {product.Name}");

        var createdProduct = await Task.Run(() => _productRepository.GetProduct(product.ProductId));
        if (createdProduct == null)
            throw new InvalidOperationException("No se pudo recuperar el producto creado.");

        return _mapper.Map<ProductDto>(createdProduct);
    }

    public async Task<bool> UpdateProductAsync(int productId, UpdateProductDto updateProductDto, string? imagePath = null)
    {
        // Validar que el producto existe
        if (!_productRepository.ProductExists(productId))
        {
            throw new InvalidOperationException("El producto no existe.");
        }

        // Validar que la categoría existe
        if (!_categoryRepository.CategoryExists(updateProductDto.CategoryId))
        {
            throw new InvalidOperationException("La categoría especificada no existe.");
        }

        var product = _mapper.Map<Product>(updateProductDto);
        product.ProductId = productId;

        // Asignar imagen si fue proporcionada
        if (!string.IsNullOrEmpty(imagePath))
        {
            product.ImgUrl = imagePath;
        }
        else if (!string.IsNullOrEmpty(updateProductDto.ImgUrl))
        {
            product.ImgUrl = updateProductDto.ImgUrl;
        }

        return await Task.Run(() => _productRepository.UpdateProduct(product));
    }

    public async Task<bool> DeleteProductAsync(int productId)
    {
        var product = await Task.Run(() => _productRepository.GetProduct(productId));
        if (product == null)
            return false;

        return await Task.Run(() => _productRepository.DeleteProduct(product));
    }

    public async Task<IEnumerable<ProductDto>> GetProductsForCategoryAsync(int categoryId)
    {
        var products = await Task.Run(() => _productRepository.GetProductsInCategory(categoryId));
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<IEnumerable<ProductDto>> SearchProductAsync(string name)
    {
        var products = await Task.Run(() => _productRepository.SearchProduct(name));
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<bool> BuyProductAsync(string name, int quantity)
    {
        var product = await Task.Run(() => _productRepository.GetProductByName(name));
        if (product == null)
            throw new InvalidOperationException("El producto con el nombre especificado no existe.");

        var result = await Task.Run(() => _productRepository.BuyProduct(name, quantity));
        if (!result)
            throw new InvalidOperationException("No hay suficiente stock para completar la compra.");

        return true;
    }

    public async Task<bool> ProductExistsAsync(int productId)
    {
        return await Task.Run(() => _productRepository.ProductExists(productId));
    }

    public async Task<bool> ProductExistsByNameAsync(string name)
    {
        return await Task.Run(() => _productRepository.ProductExists(name));
    }
}
