using System;

namespace SO_API_REST.Domain.Entities;

public class Entry
{
    public int EntryId { get; set; }
    public string Comment { get; set; }
    public DateTime Date { get; set; }
    public string Terminal { get; set; }

    // Relationship with Permission
    public int PermissionId { get; set; }
    public Permission Permission { get; set; }
}