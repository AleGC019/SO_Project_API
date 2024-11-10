using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RM_API.Core.Entities;

public enum TerminalType
{
    Vehicular,
    Peatonal
}

public class Entry
{
    [Key]
    public int EntryId { get; set; }
    
    [MaxLength(500, ErrorMessage = "El comentario no puede superar los 500 caracteres.")]
    public string? EntryComment { get; set; }
    
    [Required(ErrorMessage = "La fecha es obligatoria.")]
    public DateTime EntryTimestamp { get; set; } = DateTime.Now;
    
    [Required(ErrorMessage = "El tipo de terminal es obligatorio.")]
    [EnumDataType(typeof(TerminalType), ErrorMessage = "El tipo de terminal no es válido.")]
    public TerminalType EntryTerminal { get; set; }

    // Relación con la entidad Permiso (una entrada puede estar asociada a un permiso específico)
    [ForeignKey("Permission")]
    public int PermissionId { get; set; }
    public Permission EntryPermission { get; set; }
    
    public bool IsActive { get; set; } = true;
}