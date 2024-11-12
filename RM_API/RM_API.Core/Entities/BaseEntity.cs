using System.ComponentModel.DataAnnotations;

namespace RM_API.Core.Entities;

public class BaseEntity
{
    [Key] public Guid Id { get; set; } = Guid.NewGuid(); // Auto-generate UUID
}