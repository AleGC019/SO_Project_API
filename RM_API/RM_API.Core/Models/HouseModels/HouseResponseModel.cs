using RM_API.Core.Models.UserModels;

namespace RM_API.Core.Models.HouseModels;

public class HouseResponseModel
{
    public int HouseNumber { get; set; }
    public string Address { get; set; }
    public List<UserResponseModel> inhabitants { get; set; }
}