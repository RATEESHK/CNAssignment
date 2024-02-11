using Dapper;
using ECommerce.Api.Entities;
using ECommerce.Api.Repository.Interfaces;
using System.Data;

namespace ECommerce.Api.Repository;

public class OrderItemsRepository : IOrderItemsRepository
{
    private readonly IDbConnection _dbConnection;
    private readonly ILogger<OrderItemsRepository> _logger;

    public OrderItemsRepository(IDbConnection dbConnection, ILogger<OrderItemsRepository> logger)
    {
        _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<OrderItems>> GetOrderItems()
    {
        try
        {
            if (_dbConnection.State == ConnectionState.Closed)
            {
                _dbConnection.Open();
            }
            var orderItems = await _dbConnection.QueryAsync<OrderItems>("SELECT * FROM OrderItems");
            if (orderItems.Any())
            {
                foreach (var orderItem in orderItems)
                {
                    var order = await GetOrdersByOrderItemId(orderItem.OrderItemId);
                    var product = await GetProductsByOrderItemId(orderItem.OrderItemId);

                    orderItem.Order = order.FirstOrDefault();
                    orderItem.Product = product.FirstOrDefault();
                }
            }
            return orderItems;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetOrderItems");
            throw;
        }
        finally
        {
            _dbConnection.Close();
        }
    }

    public async Task<OrderItems> GetOrderItemById(int orderItemId)
    {
        try
        {
            if (_dbConnection.State == ConnectionState.Closed)
            {
                _dbConnection.Open();
            }
            var orderItem = await _dbConnection.QueryFirstOrDefaultAsync<OrderItems>("SELECT * FROM OrderItems WHERE OrderItemId = @OrderItemId", new { OrderItemId = orderItemId });
            if (orderItem != null)
            {
                var order = await GetOrdersByOrderItemId(orderItem.OrderItemId);
                var product = await GetProductsByOrderItemId(orderItem.OrderItemId);

                orderItem.Order = order.FirstOrDefault();
                orderItem.Product = product.FirstOrDefault();
            }
            return orderItem;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetOrderItemById");
            throw;
        }
        finally
        {
            _dbConnection.Close();
        }
    }

    public async Task<int> AddOrderItem(OrderItems orderItem)
    {
        try
        {
            if (_dbConnection.State == ConnectionState.Closed)
            {
                _dbConnection.Open();
            }
            return await _dbConnection.ExecuteAsync("INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price) VALUES (@OrderId, @ProductId, @Quantity, @Price)", orderItem);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in AddOrderItem");
            throw;
        }
        finally
        {
            _dbConnection.Close();
        }
    }

    public async Task<int> UpdateOrderItem(OrderItems orderItem)
    {
        try
        {
            if (_dbConnection.State == ConnectionState.Closed)
            {
                _dbConnection.Open();
            }
            return await _dbConnection.ExecuteAsync("UPDATE OrderItems SET OrderId = @OrderId, ProductId = @ProductId, Quantity = @Quantity, Price = @Price WHERE OrderItemId = @OrderItemId", orderItem);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in UpdateOrderItem");
            throw;
        }
        finally
        {
            _dbConnection.Close();
        }
    }

    public async Task<int> DeleteOrderItem(int orderItemId)
    {
        try
        {
            if (_dbConnection.State == ConnectionState.Closed)
            {
                _dbConnection.Open();
            }
            return await _dbConnection.ExecuteAsync("DELETE FROM OrderItems WHERE OrderItemId = @OrderItemId", new { OrderItemId = orderItemId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in DeleteOrderItem");
            throw;
        }
        finally
        {
            _dbConnection.Close();
        }
    }

    public async Task<IEnumerable<OrderItems>> GetOrderItemsByOrderId(int orderId)
    {
        try
        {
            if (_dbConnection.State == ConnectionState.Closed)
            {
                _dbConnection.Open();
            }
            var orderItems = await _dbConnection.QueryAsync<OrderItems>("SELECT * FROM OrderItems WHERE OrderId = @OrderId", new { OrderId = orderId });
            if (orderItems.Any())
            {
                foreach (var orderItem in orderItems)
                {
                    var order = await GetOrdersByOrderItemId(orderItem.OrderItemId);
                    var product = await GetProductsByOrderItemId(orderItem.OrderItemId);

                    orderItem.Order = order.FirstOrDefault();
                    orderItem.Product = product.FirstOrDefault();
                }
            }
            return orderItems;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetOrderItemsByOrderId");
            throw;
        }
        finally
        {
            _dbConnection.Close();
        }
    }

    public async Task<IEnumerable<OrderItems>> GetOrderItemsByProductId(int productId)
    {
        try
        {
            if (_dbConnection.State == ConnectionState.Closed)
            {
                _dbConnection.Open();
            }
            var orderItems = await _dbConnection.QueryAsync<OrderItems>("SELECT * FROM OrderItems WHERE ProductId = @ProductId", new { ProductId = productId });
            if (orderItems.Any())
            {
                foreach (var orderItem in orderItems)
                {
                    var order = await GetOrdersByOrderItemId(orderItem.OrderItemId);
                    var product = await GetProductsByOrderItemId(orderItem.OrderItemId);

                    orderItem.Order = order.FirstOrDefault();
                    orderItem.Product = product.FirstOrDefault();
                }
            }
            return orderItems;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetOrderItemsByProductId");
            throw;
        }
        finally
        {
            _dbConnection.Close();
        }
    }

    public async Task<IEnumerable<Orders>> GetOrdersByOrderItemId(int orderItemId)
    {
        try
        {
            if (_dbConnection.State == ConnectionState.Closed)
            {
                _dbConnection.Open();
            }
            var orders = await _dbConnection.QueryAsync<Orders>("SELECT * FROM Orders WHERE OrderId = (SELECT OrderId FROM OrderItems WHERE OrderItemId = @OrderItemId)", new { OrderItemId = orderItemId });
            return orders;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetOrdersByOrderItemId");
            throw;
        }
        finally
        {
            _dbConnection.Close();
        }
    }

    public async Task<IEnumerable<Products>> GetProductsByOrderItemId(int orderItemId)
    {
        try
        {
            if (_dbConnection.State == ConnectionState.Closed)
            {
                _dbConnection.Open();
            }
            var products = await _dbConnection.QueryAsync<Products>("SELECT * FROM Products WHERE ProductId = (SELECT ProductId FROM OrderItems WHERE OrderItemId = @OrderItemId)", new { OrderItemId = orderItemId });
            return products;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetProductsByOrderItemId");
            throw;
        }
        finally
        {
            _dbConnection.Close();
        }
    }
}
