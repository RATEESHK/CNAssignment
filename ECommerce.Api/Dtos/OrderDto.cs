namespace ECommerce.Api.Dtos;

public class OrderDto
{
    public int OrderNumber { get; set; }
    public string OrderDate { get; set; }
    public string DeliveryAddress { get; set; }
    public List<OrderItemDto> OrderItems { get; set; }
    public string DeliveryExpected { get; set; }
}
