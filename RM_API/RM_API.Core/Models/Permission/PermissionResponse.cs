namespace RM_API.Core.Models.Permission;

public class PermissionResponse
{
    public Guid PermitId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool Status { get; set; }
    public Guid HouseId { get; set; }
    public Guid UserId { get; set; }
}