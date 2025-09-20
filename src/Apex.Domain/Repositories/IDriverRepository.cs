using Apex.Domain.Entities;

namespace Apex.Domain.Repositories;

public interface IDriverRepository
{
    Task<Driver?> GetByNumberAsync(int number);
    Task<IEnumerable<Driver>?> GetAllAsync();
    Task AddAsync(Driver driver);
}