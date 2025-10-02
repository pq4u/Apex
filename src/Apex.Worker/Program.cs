using Apex.Application;
using Apex.Infrastructure;
using Apex.Infrastructure.DAL;
using Apex.Worker.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration
        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);
}

builder.Logging.AddSerilog();

builder.Services.AddApplication(builder.Configuration, includeIngestionCommands: true);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddWorkerServices(builder.Configuration);

var host = builder.Build();

if (builder.Environment.IsDevelopment())
{
    await host.MigrateDatabase();
}

await host.RunAsync();