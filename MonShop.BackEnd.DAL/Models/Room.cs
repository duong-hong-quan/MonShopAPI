using System.ComponentModel.DataAnnotations;

namespace MonShop.BackEnd.DAL.Models;

public class Room
{
    [Key] public int RoomId { get; set; }

    public string? RoomName { get; set; }
    public string? RoomImg { get; set; }
}