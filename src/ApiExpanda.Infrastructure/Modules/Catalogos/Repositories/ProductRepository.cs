using ApiExpanda.Application.Modules.Catalogos.Interfaces;
using ApiExpanda.Domain.Modules.Catalogos.Entities;
using ApiExpanda.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiExpanda.Infrastructure.Modules.Catalogos.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _db;

    public ProductRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public ICollection<Product> GetProducts()
    {
        return _db.Products.Include(p => p.Category).OrderBy(p => p.Name).ToList();
    }

    public ICollection<Product> GetProductsInCategory(int categoryId)
    {
        return _db.Products.Include(p => p.Category).Where(p => p.CategoryId == categoryId).ToList();
    }

    public ICollection<Product> SearchProduct(string name)
    {
        return _db.Products.Include(p => p.Category).Where(
            p => p.Name.ToLower().Trim().Contains(name.ToLower().Trim()) || 
                 p.Description.ToLower().Trim().Contains(name.ToLower().Trim())).ToList();
    }

    public Product? GetProduct(int id)
    {
        return _db.Products.Include(p => p.Category).FirstOrDefault(p => p.ProductId == id);
    }

    public Product? GetProductByName(string name)
    {
        return _db.Products.Include(p => p.Category).FirstOrDefault(p => p.Name.ToLower().Trim() == name.ToLower().Trim());
    }

    public bool BuyProduct(string name, int quantity)
    {
        if (string.IsNullOrWhiteSpace(name) || quantity <= 0)
        {
            return false;
        }
        var product = _db.Products.FirstOrDefault(p => p.Name.ToLower().Trim() == name.ToLower().Trim());
        if (product == null || product.Stock < quantity)
        {
            return false;
        }
        product.Stock -= quantity;
        _db.Products.Update(product);
        return Save();
    }

    public bool ProductExists(int productId)
    {
        return _db.Products.Any(c => c.ProductId == productId);
    }

    public bool ProductExists(string name)
    {
        return _db.Products.Any(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
    }

    public bool CreateProduct(Product product)
    {
        product.CreationDate = DateTime.Now;
        product.UpdateDate = DateTime.Now;
        _db.Products.Add(product);
        return Save();
    }

    public bool UpdateProduct(Product product)
    {
        product.UpdateDate = DateTime.Now;
        _db.Products.Update(product);
        return Save();
    }

    public bool DeleteProduct(Product product)
    {
        _db.Products.Remove(product);
        return Save();
    }

    public bool Save()
    {
        return _db.SaveChanges() >= 0;
    }

    public ICollection<Product> GetProductsInPages(int pageNumber, int pageSize)
    {
        return _db.Products
            .Include(p => p.Category)
            .OrderBy(p => p.ProductId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    public int GetTotalProducts()
    {
        return _db.Products.Count();
    }
}
