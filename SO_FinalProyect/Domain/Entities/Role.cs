using System.Collections.Generic;

namespace SO_API_REST.Domain.Entities;

public class Role
{
    public int RoleId { get; set; }
    public string RoleName { get; set; }

    // Relationship with User
    public ICollection<User> Users { get; set; } = new List<User>();
}