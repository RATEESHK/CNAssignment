using ECommerce.Api.Dtos;

namespace ECommerce.Api.Services.Interfaces;

public interface IOrderService
{
    Task<OrderDetailsDto> GetMostRecentOrderAsync(string email, string customerId);
}
