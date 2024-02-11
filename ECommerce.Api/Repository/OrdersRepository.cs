using Dapper;
using ECommerce.Api.Entities;
using ECommerce.Api.Repository.Interfaces;
using System.Data;

namespace ECommerce.Api.Repository;

public class OrdersRepository : IOrdersRepository
{
    private readonly IDbConnection _dbConnection;
    private readonly ILogger<OrdersRepository> _logger;

    public OrdersRepository(IDbConnection dbConnection, ILogger<OrdersRepository> logger)
    {
        _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Orders>> GetOrders()
    {
        try
        {
            if (_dbConnection.State == ConnectionState.Closed)
            {
                _dbConnection.Open();
            }
            var orders = await _dbConnection.QueryAsync<Orders>("SELECT * FROM Orders");
            if (orders.Any())
            {
                foreach (var order in orders)
                {
                    var orderItems = await GetOrderItems(order.OrderId);
                    var customer = await GetCustomerByOrder(order.OrderId);

                    order.OrderItems = orderItems.ToList<OrderItems>();
                    order.Customer = customer;
                }
            }
            return orders;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetOrders");
            throw;
        }
        finally
        {
            _dbConnection.Close();
        }
    }

    public async Task<Orders> GetOrderById(int orderId)
    {
        try
        {
            if (_dbConnection.State == ConnectionState.Closed)
            {
                _dbConnection.Open();
            }
            var order = await _dbConnection.QueryFirstOrDefaultAsync<Orders>("SELECT * FROM Orders WHERE OrderId = @OrderId", new { OrderId = orderId });
            if (order != null)
            {
                var orderItems = await GetOrderItems(order.OrderId);
                var customer = await GetCustomerByOrder(order.OrderId);

                order.OrderItems = orderItems.ToList<OrderItems>();
                order.Customer = customer;
            }
            return order;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetOrderById");
            throw;
        }
        finally
        {
            _dbConnection.Close();
        }
    }

    public async Task<int> AddOrder(Orders order)
    {
        try
        {
            if (_dbConnection.State == ConnectionState.Closed)
            {
                _dbConnection.Open();
            }
            return await _dbConnection.ExecuteAsync("INSERT INTO Orders (CustomerId, OrderDate, DeliveryExpected, ContainsGift) VALUES (@CustomerId, @OrderDate, @DeliveryExpected, @ContainsGift)", order);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in AddOrder");
            throw;
        }
        finally
        {
            _dbConnection.Close();
        }
    }

    public async Task<int> UpdateOrder(Orders order)
    {
        try
        {
            if (_dbConnection.State == ConnectionState.Closed)
            {
                _dbConnection.Open();
            }
            return await _dbConnection.ExecuteAsync("UPDATE Orders SET CustomerId = @CustomerId, OrderDate = @OrderDate, DeliveryExpected = @DeliveryExpected, ContainsGift = @ContainsGift WHERE OrderId = @OrderId", order);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in UpdateOrder");
            throw;
        }
        finally
        {
            _dbConnection.Close();
        }
    }

    public async Task<int> DeleteOrder(int orderId)
    {
        try
        {
            if (_dbConnection.State == ConnectionState.Closed)
            {
                _dbConnection.Open();
            }
            return await _dbConnection.ExecuteAsync("DELETE FROM Orders WHERE OrderId = @OrderId", new { OrderId = orderId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in DeleteOrder");
            throw;
        }
        finally
        {
            _dbConnection.Close();
        }
    }

    public async Task<IEnumerable<OrderItems>> GetOrderItems(int orderId)
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
                    var product = await _dbConnection.QueryFirstOrDefaultAsync<Products>("SELECT * FROM Products WHERE ProductId = @ProductId", new { ProductId = orderItem.ProductId });
                    orderItem.Product = product;

                    var order = await _dbConnection.QueryFirstOrDefaultAsync<Orders>("SELECT * FROM Orders WHERE OrderId = @OrderId", new { OrderId = orderItem.OrderId });
                    orderItem.Order = order;
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

    public async Task<Customers> GetCustomerByOrder(int orderId)
    {
        try
        {
            if (_dbConnection.State == ConnectionState.Closed)
            {
                _dbConnection.Open();
            }
            var customer = await _dbConnection.QueryFirstOrDefaultAsync<Customers>("SELECT * FROM Customers WHERE CustomerId = (SELECT CustomerId FROM Orders WHERE OrderId = @OrderId)", new { OrderId = orderId });

            if (customer != null)
            {
                var orders = await _dbConnection.QueryAsync<Orders>("SELECT * FROM Orders WHERE CustomerId = @CustomerId", new { CustomerId = customer.CustomerId });
                customer.Orders = orders.ToList<Orders>();
            }
            return customer;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetCustomerByOrder");
            throw;
        }
        finally
        {
            _dbConnection.Close();
        }
    }

    public async Task<Orders> GetMostRecentOrderByCustomerIdAsync(string customerId)
    {
        try
        {
            if (_dbConnection.State == ConnectionState.Closed)
            {
                _dbConnection.Open();
            }
            var order = await _dbConnection.QueryFirstOrDefaultAsync<Orders>("SELECT * FROM Orders WHERE CustomerId = @CustomerId ORDER BY OrderDate DESC", new { CustomerId = customerId });
            if (order != null)
            {
                var orderItems = await GetOrderItems(order.OrderId);
                var customer = await GetCustomerByOrder(order.OrderId);

                order.OrderItems = orderItems.ToList<OrderItems>();
                order.Customer = customer;
            }
            return order;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetMostRecentOrderByCustomerIdAsync");
            throw;
        }
        finally
        {
            _dbConnection.Close();
        }
    }

    public async Task<IEnumerable<OrderItems>> GetOrderItemsByOrderIdAsync(int orderId)
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
                    var product = await _dbConnection.QueryFirstOrDefaultAsync<Products>("SELECT * FROM Products WHERE ProductId = @ProductId", new { ProductId = orderItem.ProductId });
                    orderItem.Product = product;

                    var order = await _dbConnection.QueryFirstOrDefaultAsync<Orders>("SELECT * FROM Orders WHERE OrderId = @OrderId", new { OrderId = orderItem.OrderId });
                    orderItem.Order = order;
                }
            }
            return orderItems;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetOrderItemsByOrderIdAsync");
            throw;
        }
        finally
        {
            _dbConnection.Close();
        }
    }
}
