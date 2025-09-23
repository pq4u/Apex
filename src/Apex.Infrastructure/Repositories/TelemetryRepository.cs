using Apex.Domain.Repositories;
using Apex.Domain.TimeSeries;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Dapper;
using Serilog;

namespace Apex.Infrastructure.Repositories;

public class TelemetryRepository : ITelemetryRepository
{
    private readonly string _connectionString;

    public TelemetryRepository(IConfiguration configuration)
        => _connectionString = configuration.GetConnectionString("ApexDb")!;

    public async Task BulkInsertCarDataAsync(List<TelemetryData> carDataList, CancellationToken cancellationToken)
    {
        if (!carDataList.Any()) return;

        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        NpgsqlBinaryImporter? writer = null;
        try
        {
            writer = await connection.BeginBinaryImportAsync(
                "COPY telemetry.car_data (time, session_id, driver_id, speed, rpm, gear, throttle, brake, drs, n_gear) FROM STDIN (FORMAT BINARY)");

            foreach (var data in carDataList)
            {
                await writer.StartRowAsync();
                await writer.WriteAsync(data.Time, NpgsqlTypes.NpgsqlDbType.TimestampTz);
                await writer.WriteAsync(data.SessionId, NpgsqlTypes.NpgsqlDbType.Integer);
                await writer.WriteAsync(data.DriverId, NpgsqlTypes.NpgsqlDbType.Integer);
                await writer.WriteAsync(data.Speed, NpgsqlTypes.NpgsqlDbType.Smallint);
                await writer.WriteAsync(data.Rpm, NpgsqlTypes.NpgsqlDbType.Smallint);
                await writer.WriteAsync(data.Gear, NpgsqlTypes.NpgsqlDbType.Smallint);
                await writer.WriteAsync(data.Throttle, NpgsqlTypes.NpgsqlDbType.Smallint);
                await writer.WriteAsync(data.Brake, NpgsqlTypes.NpgsqlDbType.Smallint);
                await writer.WriteAsync(data.Drs, NpgsqlTypes.NpgsqlDbType.Smallint);
                await writer.WriteAsync(data.NGear, NpgsqlTypes.NpgsqlDbType.Smallint);
            }

            await writer.CompleteAsync(cancellationToken);
            Log.Information("Bulk inserted {Count} car data records", carDataList.Count);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to bulk insert {Count} car data records", carDataList.Count);
            // automatically dispose from NpqsqlBinaryImporter
            throw;
        }
        finally
        {
            writer?.Dispose();
        }
    }

    public async Task<long> GetCarDataCountAsync(int sessionId, int driverId)
    {
        try
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var sql = @"
                    SELECT COUNT(*) 
                    FROM telemetry.car_data 
                    WHERE session_id = @SessionId AND driver_id = @DriverId";

            return await connection.ExecuteScalarAsync<long>(sql, new { SessionId = sessionId, DriverId = driverId });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to get car data count for session {SessionId}, driver {DriverId}", sessionId, driverId);
            throw;
        }
    }

    public async Task<IEnumerable<TelemetryData>> GetCarDataAsync(int sessionId, int driverId)
    {
        try
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
                    SELECT time, session_id, driver_id, speed, rpm, gear, throttle, brake, drs, n_gear
                    FROM telemetry.car_data
                    WHERE session_id = @SessionId AND driver_id = @DriverId";

            sql += " ORDER BY time";

            return await connection.QueryAsync<TelemetryData>(sql, new
            {
                SessionId = sessionId,
                DriverId = driverId
            });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to get car data for session {SessionId}, driver {DriverId}", sessionId, driverId);
            throw;
        }
    }
}