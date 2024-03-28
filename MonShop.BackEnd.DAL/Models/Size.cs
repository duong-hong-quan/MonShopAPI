using System.ComponentModel.DataAnnotations;

namespace MonShop.BackEnd.DAL.Models;

public class Size
{
    [Key] public int SizeId { get; set; }

    public string SizeName { get; set; }
}