using ConsoleApp;
using Domain.Entities;
using Domain.Services;
using Infrastructure.Servicies;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(ConfigureServices)
    .Build();

var svc = ActivatorUtilities.CreateInstance<EntryPoint>(host.Services);

svc.MainRun();

static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
{
    services
        .AddTransient<IRobot, Robot>()
        .AddTransient<IReportService, ReportService>()
        .AddTransient<RobotClient>();
}
