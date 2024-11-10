using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RM_API.Core.Entities;

public class Permission
{
    [Key]
    public int PermissionId { get; set; }
    
    [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
    public DateTime StartDate { get; set; }
    [Required(ErrorMessage = "La fecha de fin es obligatoria.")]
    public DateTime EndDate { get; set; }
    
    [Required(ErrorMessage = "El estado del permiso es obligatorio.")]
    public bool Status { get; set; }

    // Relación con la entidad Casa (muchos permisos pueden pertenecer a una casa)
    [ForeignKey("Casa")]
    public int HouseId { get; set; }
    public House PermissionHouse { get; set; }

    // Relación con la entidad Usuario (muchos permisos pueden pertenecer a un usuario)
    [ForeignKey("Usuario")]
    public int UserId { get; set; }
    public User PermissionUser { get; set; }
    
    public bool IsActive { get; set; } = true;
}