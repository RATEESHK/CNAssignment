using ECommerce.Api.Entities;

namespace ECommerce.Api.Repository.Interfaces
{
    public interface IOrderItemsRepository
    {
        Task<IEnumerable<OrderItems>> GetOrderItems();
        Task<OrderItems> GetOrderItemById(int orderItemId);
        Task<int> AddOrderItem(OrderItems orderItem);
        Task<int> UpdateOrderItem(OrderItems orderItem);
        Task<int> DeleteOrderItem(int orderItemId);
        Task<IEnumerable<OrderItems>> GetOrderItemsByOrderId(int orderId);
        Task<IEnumerable<OrderItems>> GetOrderItemsByProductId(int productId);
        Task<IEnumerable<Orders>> GetOrdersByOrderItemId(int orderItemId);
        Task<IEnumerable<Products>> GetProductsByOrderItemId(int orderItemId);
    }
}
