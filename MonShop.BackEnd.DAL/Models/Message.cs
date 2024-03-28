using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonShop.BackEnd.DAL.Models;

public class Message
{
    [Key] public int MessageId { get; set; }

    public string? Content { get; set; }
    public DateTime? SendTime { get; set; }

    public int RoomMemberChatId { get; set; }

    [ForeignKey(nameof(RoomMemberChatId))] public RoomMemberChat memberChat { get; set; }
}