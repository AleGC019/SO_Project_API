using RM_API.Core.Entities;

namespace RM_API.Data.Repositories.Interfaces;

public interface IHouseRepository
{
    Task SaveHouse(House house);
    Task<House?> GetHouseById(Guid id);
    Task<House?> GetHouseByHouseNumber(int houseNumber);
}