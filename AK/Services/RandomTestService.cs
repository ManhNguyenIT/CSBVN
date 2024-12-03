using AK.Enums;
using Microsoft.Extensions.Hosting;

namespace AK.Services;

public class RandomTestService : IHostedService
{
    private readonly DeviceStateManager _deviceStateManager;
    private readonly DataSendService _dataSendService;
    private readonly ComDataService _comDataService;
    private readonly System.Timers.Timer _timer = new()
    {
        Interval = 5000,
    };


    private readonly Random random = new();
    private readonly Array TargetConnectStateValues = Enum.GetValues(typeof(TargetConnectState));

    public RandomTestService(DeviceStateManager deviceStateManager, DataSendService dataSendService, ComDataService comDataService)
    {
        _deviceStateManager = deviceStateManager;
        _dataSendService = dataSendService;
        _comDataService = comDataService;

        _timer.Elapsed += _timer_Elapsed;
    }

    private void _timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
    {


    }



    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer.Start();
        _comDataService.Open("COM16", 57600);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer.Stop();
        return Task.CompletedTask;
    }
}
