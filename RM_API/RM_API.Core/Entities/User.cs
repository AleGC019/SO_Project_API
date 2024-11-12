using System.ComponentModel.DataAnnotations;

namespace RM_API.Core.Entities;

public class User : BaseEntity
{
    [Required(ErrorMessage = "El nombre del usuario es obligatorio.")]
    [MaxLength(10, ErrorMessage = "El nombre no puede superar los 10 caracteres.")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [MaxLength(30, ErrorMessage = "El correo no puede superar los 25 caracteres.")]
    public string UserEmail { get; set; }

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    public string UserPassword { get; set; }

    public bool IsActive { get; set; } = true;

    // Relationship with Role (a user can only have one role, but a role can have multiple users)
    public Guid RoleId { get; set; }
    public Role UserRole { get; set; }

    // Relationship with Permissions (a user can have multiple permissions, but a permission can only be associated with one user)
    public List<Permission>? Permissions { get; set; } = new();

    // Relationship with House (a user can only line in one house, but a house can have multiple users)
    public Guid? HouseId { get; set; }
    public virtual House? UserHouse { get; set; }
}