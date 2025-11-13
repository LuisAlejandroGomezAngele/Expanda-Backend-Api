using System;
using ApiExpanda.Domain.Entities;

namespace ApiExpanda.Application.Interfaces
{
    public interface IProductRepository
    {
        ICollection<Product> GetProducts();

        ICollection<Product> GetProductsInPages(int pageNumber, int pageSize);

        int GetTotalProducts();

        ICollection<Product> GetProductsInCategory(int categoryId);

        ICollection<Product> SearchProduct(string name);

        Product? GetProduct(int id);

        Product? GetProductByName(string name);

        bool BuyProduct(string name, int quantity);

        bool ProductExists(int id);

        bool ProductExists(string name);

        bool CreateProduct(Product product);

        bool UpdateProduct(Product product);

        bool DeleteProduct(Product product);

        bool Save();
    }
}
