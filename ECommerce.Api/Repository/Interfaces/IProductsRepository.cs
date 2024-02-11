using ECommerce.Api.Entities;

namespace ECommerce.Api.Repository.Interfaces;

public interface IProductsRepository
{
    Task<IEnumerable<Products>> GetProducts();
    Task<Products> GetProductById(int productId);
    Task<int> AddProduct(Products product);
    Task<int> UpdateProduct(Products product);
    Task<int> DeleteProduct(int productId);
}
