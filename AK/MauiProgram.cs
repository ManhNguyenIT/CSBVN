using AK.Data;
using AK.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using System.Reflection;

namespace AK
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream("AK.appsettings.json");
            var config = new ConfigurationBuilder()
                .AddJsonStream(stream!)
                .Build();
            builder.Configuration.AddConfiguration(config);

            builder.Services.Configure<IDataSettingService>(
                builder.Configuration.GetSection("AppOptions"));

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite($"Data Source=database.db");
            });
            builder.Services.AddSingleton<ComDataService>();
            builder.Services.AddSingleton<DataSendService>();
            builder.Services.AddSingleton<DataSettingService>();
            builder.Services.AddSingleton<IComDataService>(sp => sp.GetRequiredService<ComDataService>());
            builder.Services.AddSingleton<IDataSendService>(sp => sp.GetRequiredService<DataSendService>());
            builder.Services.AddSingleton<IDataSettingService>(sp => sp.GetRequiredService<DataSettingService>());
            builder.Services.AddSingleton<DeviceStateManager>();
            builder.Services.AddSingleton<ITargetConnectStateManager>(sp => sp.GetRequiredService<DeviceStateManager>());
            builder.Services.AddSingleton<ILightController>(sp => sp.GetRequiredService<DeviceStateManager>());
            builder.Services.AddSingleton<ITransmitterDeviceManager>(sp => sp.GetRequiredService<DeviceStateManager>());
            builder.Services.AddSingleton<IPageNavigationService>(sp => sp.GetRequiredService<DeviceStateManager>());

            builder.Services.AddSingleton<MissionManager>();
            builder.Services.AddSingleton<IHostedService, Worker>();
            builder.Services.AddSingleton<IHostedService, RandomTestService>();
            builder.Services.AddSingleton<HostedServiceExecutor>();

            builder.Services.AddMudServices();
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
