using Apex.Domain.Entities;
using Apex.Domain.Repositories;
using Apex.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;

namespace Apex.Infrastructure.Repositories;

public class DriverRepository : IDriverRepository
{
    private readonly ApexDbContext _dbContext;
    private readonly DbSet<Driver> _drivers;

    public DriverRepository(ApexDbContext dbContext)
    {
        _dbContext = dbContext;
        _drivers = _dbContext.Drivers;
    }

    public async Task<Driver?> GetByNumberAsync(int number)
        => await _drivers.FirstOrDefaultAsync(x => x.DriverNumber == number);

    public async Task<IEnumerable<Driver>?> GetAllAsync()
        => await _drivers.ToListAsync();

    public async Task AddAsync(Driver driver)
    {
        await _drivers.AddAsync(driver);
    }
}