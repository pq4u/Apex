using Polly;
using Polly.Extensions.Http;
using Serilog;

namespace Apex.Worker.Configuration;

public static class HttpClientPolicies
{
    public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() => HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),     // 2, 4, 8 sec
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    Log.Warning("HTTP retry {RetryCount} after {Delay}ms for {Context}", retryCount, timespan.TotalMilliseconds, context.OperationKey);
                });

    public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy() => HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 3,
                durationOfBreak: TimeSpan.FromSeconds(30),
                onBreak: (result, timespan) =>
                {
                    Log.Warning("Circuit breaker opened for {Duration}ms", timespan.TotalMilliseconds);
                },
                onReset: () =>
                {
                    Log.Information("Circuit breaker reset");
                });
}