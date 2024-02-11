namespace ECommerce.Api.Entities;

public class Orders
{
    public int OrderId { get; set; }

    public string CustomerId { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime DeliveryExpected { get; set; }

    public bool ContainsGift { get; set; }

    public virtual Customers Customer { get; set; }
    public virtual ICollection<OrderItems> OrderItems { get; set; }
}
