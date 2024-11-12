using System.ComponentModel.DataAnnotations;

namespace RM_API.Core.Entities;

public class Permission : BaseEntity
{
    [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "La fecha de fin es obligatoria.")]
    public DateTime EndDate { get; set; }

    [Required(ErrorMessage = "El estado del permiso es obligatorio.")]
    public bool Status { get; set; }

    // Relationships: A permission must have a house and user associated with it, it can not exist without them

    // Relationship with House (a permission can only be associated with one house, but a house can have multiple permissions)
    public Guid HouseId { get; set; }
    public House PermissionHouse { get; set; }

    // Relationship with User (a permission can only be associated with one user, but a user can have multiple permissions)
    public Guid UserId { get; set; }
    public User PermissionUser { get; set; }

    public bool IsActive { get; set; } = true;
}