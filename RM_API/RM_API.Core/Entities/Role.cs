using System.ComponentModel.DataAnnotations;

namespace RM_API.Core.Entities;

public enum RoleName
{
    Administrador,
    Vigilante,
    Residente
}

public class Role
{
    [Key]
    public int RoleId { get; set; }
    
    [Required(ErrorMessage = "El nombre del rol es obligatorio.")]
    [MaxLength(50, ErrorMessage = "El nombre del rol no puede superar los 50 caracteres.")]
    [EnumDataType(typeof(RoleName), ErrorMessage = "El nombre del rol no es válido.")]
    public RoleName RoleName { get; set; }
    
    public virtual List<User> Usuarios { get; set; } = new List<User>();
    
    public bool IsActive { get; set; } = true;
}