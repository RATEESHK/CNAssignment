﻿namespace ECommerce.Api.Entities;

public class OrderItems
{
    public int OrderItemId { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public virtual Orders Order { get; set; }
    public virtual Products Product { get; set; }
}
