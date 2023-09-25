using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MonShop.Library.Models
{
    public  class ProductStatus
    {
        [Key]

        public int ProductStatusId { get; set; }
        public string Status { get; set; } = null!;

    }
}
