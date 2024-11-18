using System.Text.Json.Serialization;

namespace RM_API.Core.Models.EntryModels;

public class EntryResponseModel
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? EntryComment { get; set; }
    public DateTime EntryTimestamp { get; set; }
    public string EntryTerminal { get; set; } 
    public string PermissionId { get; set; }
}