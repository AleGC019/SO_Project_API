using System.ComponentModel.DataAnnotations;

namespace RM_API.Core.Models.HouseModels;

public class NewHouseModel
{
    [Required(ErrorMessage = "House number is required")]
    [Range(1, 9999, ErrorMessage = "House number must be greater than 0 and below 9999")]
    public int houseNumber { get; set; }

    [Required(ErrorMessage = "House address is required")]
    [MaxLength(50, ErrorMessage = "House address cannot exceed 50 characters")]
    public string address { get; set; }
}