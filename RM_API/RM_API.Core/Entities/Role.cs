using System.ComponentModel.DataAnnotations;

namespace RM_API.Core.Entities;

public enum RoleName
{
    ADMIN,
    SEC,
    RES
}

public class Role : BaseEntity
{
    [Required(ErrorMessage = "El nombre del rol es obligatorio.")]
    [MaxLength(5, ErrorMessage = "El nombre del rol no puede superar los 50 caracteres.")]
    [EnumDataType(typeof(RoleName), ErrorMessage = "El nombre del rol brindado no es válido.")]
    public RoleName RoleName { get; set; }

    // A role can have multiple users, but each user can only have one role
    public List<User>? Users { get; set; } = new();

    public bool IsActive { get; set; } = true;
}