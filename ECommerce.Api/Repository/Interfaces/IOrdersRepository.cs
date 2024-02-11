using ECommerce.Api.Entities;

namespace ECommerce.Api.Repository.Interfaces;

public interface IOrdersRepository
{
    Task<IEnumerable<Orders>> GetOrders();
    Task<Orders> GetOrderById(int orderId);
    Task<int> AddOrder(Orders order);
    Task<int> UpdateOrder(Orders order);
    Task<int> DeleteOrder(int orderId);
    Task<IEnumerable<OrderItems>> GetOrderItems(int orderId);
    Task<Customers> GetCustomerByOrder(int orderId);
    Task<Orders> GetMostRecentOrderByCustomerIdAsync(string customerId);
    Task<IEnumerable<OrderItems>> GetOrderItemsByOrderIdAsync(int orderId);
}
