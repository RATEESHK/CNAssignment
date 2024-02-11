using AutoMapper;
using ECommerce.Api.Dtos;
using ECommerce.Api.Entities;
using ECommerce.Api.Repository.Interfaces;
using ECommerce.Api.Services.Interfaces;

namespace ECommerce.Api.Services;

public class OrderService : IOrderService
{
    private readonly ILogger<OrderService> _logger;
    private readonly IMapper _mapper;
    private readonly IOrdersRepository _orderRepository;
    private readonly ICustomersRepository _customersRepository;

    public OrderService(ILogger<OrderService> logger, IMapper mapper, IOrdersRepository orderRepository, ICustomersRepository customersRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _customersRepository = customersRepository ?? throw new ArgumentNullException(nameof(customersRepository));
    }

    public async Task<OrderDetailsDto> GetMostRecentOrderAsync(string email, string customerId)
    {
        var customer = await _customersRepository.GetCustomerByEmailAndIdAsync(email, customerId);
        if (customer == null)
        {
            throw new ArgumentException("Customer does not exist or email does not match.");
        }

        var recentOrder = await _orderRepository.GetMostRecentOrderByCustomerIdAsync(customer.CustomerId);
        if (recentOrder == null)
        {
            return new OrderDetailsDto
            {
                Customer = _mapper.Map<CustomerDto>(customer),
                Order = null // Handle no orders case
            };
        }

        var orderItems = await _orderRepository.GetOrderItemsByOrderIdAsync(recentOrder.OrderId);
        var orderDto = _mapper.Map<OrderDto>(recentOrder);
        orderDto.OrderItems = _mapper.Map<List<OrderItemDto>>(orderItems);
        orderDto.DeliveryAddress = $"{customer.HouseNo} {customer.Street}, {customer.Town}, {customer.PostCode}";

        return new OrderDetailsDto
        {
            Customer = _mapper.Map<CustomerDto>(customer),
            Order = orderDto
        };
    }
}
