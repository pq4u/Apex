using Apex.Application.Abstractions;
using Apex.Application.Client;
using Apex.Application.Mappings;
using Apex.Domain.Configuration;
using Apex.Domain.Entities;
using Apex.Domain.Repositories;
using Microsoft.Extensions.Options;
using Serilog;

namespace Apex.Application.Commands.Stints;

public class IngestStintsCommandHandler : ICommandHandler<IngestStintsCommand>
{
    private readonly IOpenF1ApiClient _apiClient;
    private readonly IStintRepository _stintRepository;
    private readonly IDriverRepository _driverRepository;
    private readonly IngestionOptions _options;

    public IngestStintsCommandHandler(IOpenF1ApiClient apiClient, IStintRepository stintRepository,
        IDriverRepository driverRepository, IOptions<IngestionOptions> options)
    {
        _apiClient = apiClient;
        _stintRepository = stintRepository;
        _driverRepository = driverRepository;
        _options = options.Value;
    }

    public async Task HandleAsync(IngestStintsCommand command)
    {
        Log.Information("Starting stints ingestion in session key {SessionKey} from OpenF1 API", command.SessionKey);
        try
        {
            var stintsExists = await _stintRepository.GetBySessionIdAsync(command.SessionKey);

            var dbDrivers = await _driverRepository.GetAllAsync();
            
            if (stintsExists!.Any())
            {
                Log.Information("Stints already exist in session key {SessionKey}", command.SessionKey);
                return;
            }
            
            var stints = await _apiClient.GetStintsAsync(command.SessionKey);

            if (!stints!.Any())
            {
                // not found in api
            }
            
            var stintsEntites = stints.Select(x => x.ToEntity(
                command.SessionId, dbDrivers!.Where(d => d.DriverNumber == x.Driver_Number)!.First().Id));

            await _stintRepository.AddRangeAsync(stintsEntites);

            Log.Information("Completed stints ingestion from OpenF1 API");
            
            await Task.Delay(_options.ApiDelayMs);
        }
        catch (Exception ex)
        {
            Log.Error("Ingesting stints failed in session key {SessionKey} with error {ErrorMessage}",
                command.SessionKey, ex.Message);
        }
    }
}