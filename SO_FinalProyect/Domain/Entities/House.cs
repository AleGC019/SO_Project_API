namespace SO_API_REST.Domain.Entities;

public class House
{
    public int HouseId { get; set; }
    public int HouseNumber { get; set; }
    public string Address { get; set; }

    // One-to-many relationship: A house can have many inhabitants (Users)
    public ICollection<User> Inhabitants { get; set; } = new List<User>();
}