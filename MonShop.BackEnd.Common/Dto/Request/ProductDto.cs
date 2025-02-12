﻿using Microsoft.AspNetCore.Http;

namespace MonShop.BackEnd.Common.Dto.Request;

public class ProductDto
{
    public int? ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public IFormFile ImageUrl { get; set; } = null!;
    public double Price { get; set; }
    public double? Discount { get; set; }
    public string? Description { get; set; }
    public int? CategoryId { get; set; }

    public int? ProductStatusId { get; set; }
    public IEnumerable<Inventory> Inventory { get; set; }
}

public class Inventory
{
    public int Quantity { get; set; }
    public int SizeId { get; set; }
}