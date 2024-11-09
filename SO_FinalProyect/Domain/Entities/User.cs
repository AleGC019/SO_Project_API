namespace SO_API_REST.Domain.Entities;
using System.Collections.Generic;
public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }

        // Relationship with Role
        public int RoleId { get; set; }
        public Role Role { get; set; }

        // Many-to-many relationship with Permission
        public ICollection<Permission> Permissions { get; set; } = new List<Permission>();

        // Many-to-one relationship with House
        public int HouseId { get; set; }
        public House House { get; set; }
    }