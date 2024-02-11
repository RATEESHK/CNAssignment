using Dapper;
using ECommerce.Api.Entities;
using ECommerce.Api.Repository.Interfaces;
using System.Data;

namespace ECommerce.Api.Repository;

public class ProductsRepository : IProductsRepository
{
    private readonly ILogger<ProductsRepository> _logger;
    private readonly IDbConnection _dbConnection;

    public ProductsRepository(IDbConnection dbConnection, ILogger<ProductsRepository> logger)
    {
        _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Products>> GetProducts()
    {
        try
        {
            if (_dbConnection.State == ConnectionState.Closed)
            {
                _dbConnection.Open();
            }
            return await _dbConnection.QueryAsync<Products>("SELECT * FROM Products");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetProducts");
            throw;
        }
        finally
        {
            _dbConnection.Close();
        }
    }

    public async Task<Products> GetProductById(int productId)
    {
        try
        {
            if (_dbConnection.State == ConnectionState.Closed)
            {
                _dbConnection.Open();
            }
            return await _dbConnection.QueryFirstOrDefaultAsync<Products>("SELECT * FROM Products WHERE ProductId = @ProductId", new { ProductId = productId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetProductById");
            throw;
        }
        finally
        {
            _dbConnection.Close();
        }
    }

    public async Task<int> AddProduct(Products product)
    {
        try
        {
            if (_dbConnection.State == ConnectionState.Closed)
            {
                _dbConnection.Open();
            }
            return await _dbConnection.ExecuteAsync("INSERT INTO Products (ProductName, Color, Size) VALUES (@ProductName, @Color, @Size)", product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in AddProduct");
            throw;
        }
        finally
        {
            _dbConnection.Close();
        }
    }

    public async Task<int> UpdateProduct(Products product)
    {
        try
        {
            if (_dbConnection.State == ConnectionState.Closed)
            {
                _dbConnection.Open();
            }
            return await _dbConnection.ExecuteAsync("UPDATE Products SET ProductName = @ProductName, Color = @Color, Size = @Size WHERE ProductId = @ProductId", product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in UpdateProduct");
            throw;
        }
        finally
        {
            _dbConnection.Close();
        }
    }

    public async Task<int> DeleteProduct(int productId)
    {
        try
        {
            if (_dbConnection.State == ConnectionState.Closed)
            {
                _dbConnection.Open();
            }
            return await _dbConnection.ExecuteAsync("DELETE FROM Products WHERE ProductId = @ProductId", new { ProductId = productId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in DeleteProduct");
            throw;
        }
        finally
        {
            _dbConnection.Close();
        }
    }
}
