using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RM_API.Core.Entities;

public class User
{
    [Key]
    public int UserId { get; set; }
    
    [Required(ErrorMessage = "El nombre del usuario es obligatorio.")]
    [MaxLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
    public string UserName { get; set; }
    
    
    [Required(ErrorMessage = "El correo es obligatorio.")]
    [MaxLength(100, ErrorMessage = "El correo no puede superar los 100 caracteres.")]
    public string UserEmail { get; set; }
    
    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [MaxLength(100, ErrorMessage = "La contraseña no puede superar los 100 caracteres.")]
    public string UserPassword { get; set; }
    
    [Required(ErrorMessage = "La edad es obligatoria.")]
    [Range(1, 120, ErrorMessage = "La edad debe ser mayor a 0 y menor a 120.")]
    public int UserAge { get; set; }

    [ForeignKey("Role")]
    public int RoleId { get; set; }
    public Role UserRole { get; set; }

    // Relación con Permiso (uno a muchos)
    public List<Permission> Permissions { get; set; } = new List<Permission>();
    
    // Relación con Casa (muchos a uno)
    [ForeignKey("House")]
    public int HouseId { get; set; }
    public virtual House UserHouse { get; set; }
    
    public bool IsActive { get; set; } = true;
}