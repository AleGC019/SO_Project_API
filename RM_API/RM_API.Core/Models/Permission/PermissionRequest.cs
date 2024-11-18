namespace RM_API.Core.Models.Permission;

public class PermissionRequest
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid HouseId { get; set; }
    public Guid UserId { get; set; }
}