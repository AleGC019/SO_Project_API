using RM_API.Core.Entities;

namespace RM_API.Data.Repositories.Interfaces;

public interface IPermitRepository
{
    Task<List<Permission>?> GetAllPermits();
    Task CreatePermit(Permission permission);
    Task DeletePermit(Guid permitId);
    Task<Permission?> GetPermitById(Guid permit);
    Task UpdatePermit(Permission permission);
    Task<List<Permission>?> GetPermitsByHouseId(Guid userId);
    Task<Permission?> GetValidPermit(Guid userId, Guid houseId);
}