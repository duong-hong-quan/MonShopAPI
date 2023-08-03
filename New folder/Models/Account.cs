


using System.Text.Json.Serialization;

namespace MonShopLibrary.Models;

public partial class Account
{
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Image { get; set; }

    public string? Fullname { get; set; }

    public string? Address { get; set; }

    public string? PhoneNumber { get; set; }

    public bool? IsDeleted { get; set; }

    public int RoleId { get; set; }

    [JsonIgnore]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [JsonIgnore]


    public virtual Role Role { get; set; } = null!;
}
