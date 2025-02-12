﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonShop.BackEnd.DAL.Models;

public class ProductInventory
{
    [Key] [Column(Order = 1)] public int ProductId { get; set; }

    [ForeignKey("ProductId")] public Product Product { get; set; }

    [Key] [Column(Order = 2)] public int SizeId { get; set; }

    [ForeignKey("SizeId")] public Size Size { get; set; }

    public int Quantity { get; set; }
}