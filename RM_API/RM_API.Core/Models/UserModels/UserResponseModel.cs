using System.Text.Json.Serialization;

namespace RM_API.Core.Models.UserModels;

public class UserResponseModel
{
    public string username { get; set; }
    public string email { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? role { get; set; }
}