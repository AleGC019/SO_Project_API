using System.ComponentModel.DataAnnotations;

namespace RM_API.Core.Entities;

public enum TerminalType
{
    Vehicular,
    Peatonal
}

public class Entry : BaseEntity
{
    [MaxLength(50, ErrorMessage = "El comentario no puede superar los 50 caracteres.")]
    public string? EntryComment { get; set; }

    [Required(ErrorMessage = "La fecha es obligatoria.")]
    public DateTime EntryTimestamp { get; set; } = DateTime.UtcNow;

    [Required(ErrorMessage = "El tipo de terminal es obligatorio.")]
    [EnumDataType(typeof(TerminalType), ErrorMessage = "El tipo de terminal no es válido.")]
    public TerminalType EntryTerminal { get; set; }

    // Relationship with Permission (an entry can only have one permission, but a permission can have multiple entries as long as they are not inactive)
    public Guid PermissionId { get; set; }
    public virtual Permission EntryPermission { get; set; }

    public bool IsActive { get; set; } = true;
}