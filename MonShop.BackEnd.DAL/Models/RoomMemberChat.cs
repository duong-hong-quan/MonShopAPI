using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonShop.BackEnd.DAL.Models;

public class RoomMemberChat
{
    [Key] public int RoomMemberChatID { get; set; }

    public int RoomID { get; set; }

    [ForeignKey(nameof(RoomID))] public Room Room { get; set; }

    public string UserId { get; set; }

    [ForeignKey(nameof(UserId))] public ApplicationUser User { get; set; }
}