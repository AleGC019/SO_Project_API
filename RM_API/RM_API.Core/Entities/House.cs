using System.ComponentModel.DataAnnotations;

namespace RM_API.Core.Entities;

public class House : BaseEntity
{
    [Required(ErrorMessage = "El número de casa es requerido")]
    [Range(1, 9999, ErrorMessage = "El número de casa debe ser mayor a 0 y menor a 9999")]
    public int HouseNumber { get; set; }

    [Required(ErrorMessage = "La dirección de la casa es requerida")]
    [MaxLength(50, ErrorMessage = "La dirección de la casa no puede exceder los 50 caracteres")]
    public string HouseAddress { get; set; }

    // Relationship with User (a house can have multiple inhabitants, but a user can only live in one house)
    public List<User>? Inhabitants { get; set; } = new();

    public bool IsActive { get; set; } = true;
}