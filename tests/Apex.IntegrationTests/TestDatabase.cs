using Apex.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;

namespace Apex.IntegrationTests;

internal class TestDatabase : IDisposable
{
    public ApexDbContext Context { get; }

    public TestDatabase()
    {
        var options = new OptionsProvider().GetOptions<PostgresOptions>("postgres");
        Context = new ApexDbContext(new DbContextOptionsBuilder<ApexDbContext>().UseNpgsql(options.ConnectionString).Options);
    }
    
    public void Dispose()
    {
        Context.Database.EnsureDeleted();
        Context.Dispose();
    }
}