using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MonShop.BackEnd.DAL.Models
{
    public class ProductStatus
    {
        [Key]

        public int ProductStatusId { get; set; }
        public string Status { get; set; } = null!;

    }
}
