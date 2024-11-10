using System.ComponentModel.DataAnnotations;

namespace RM_API.Core.Entities;

public class House
{
    [Key]
    public int HouseId { get; set; }
    
    [Required(ErrorMessage = "El número de casa es requerido")]
    [Range(1, 9999, ErrorMessage = "El número de casa debe ser mayor a 0 y menor a 9999")]
    public int HouseNumber { get; set; }
    
    [Required(ErrorMessage = "La dirección de la casa es requerida")]
    [MaxLength(100, ErrorMessage = "La dirección de la casa no puede exceder los 100 caracteres")]
    public string HouseAddress { get; set; }

    // Relación uno a muchos: Una casa puede tener varios habitantes
    public List<User> Inhabitants { get; set; } = new List<User>();
    
    public bool IsActive { get; set; } = true;
}