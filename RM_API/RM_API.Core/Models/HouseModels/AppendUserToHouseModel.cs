using System.ComponentModel.DataAnnotations;

namespace RM_API.Core.Models.HouseModels;

public class AppendUserToHouseModel
{
    [Required(ErrorMessage = "User email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string UserEmail { get; set; }
    
    [Required(ErrorMessage = "House number is required")]
    public int HouseNumber { get; set; }
}