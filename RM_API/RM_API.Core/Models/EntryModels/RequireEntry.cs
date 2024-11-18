using System.Text.Json.Serialization;
using RM_API.Core.Entities;

namespace RM_API.Core.Models.EntryModels;

public class RequireEntry
{
    public DateTime? Date { get; set; } = DateTime.UtcNow;
    public string? Description { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public Guid HouseId { get; set; }
    public TerminalType Terminal { get; set; }
}