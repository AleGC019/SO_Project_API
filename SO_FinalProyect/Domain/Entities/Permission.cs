using System;

namespace SO_API_REST.Domain.Entities;

public class Permission
{
    public int PermissionId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    // Relationship with House
    public int HouseId { get; set; }
    public House House { get; set; }

    // Relationship with User
    public int UserId { get; set; }
    public User User { get; set; }
}