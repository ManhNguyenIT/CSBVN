using Microsoft.Extensions.Hosting;

namespace MARM
{
    public class HostedServiceExecutor
    {
        private readonly IEnumerable<IHostedService> _hostedServices;
        public HostedServiceExecutor(IEnumerable<IHostedService> hostedServices)
        {
            _hostedServices = hostedServices;
        }
        public async Task StartAllAsync(CancellationToken cancellationToken)
        {
            foreach (var hostedService in _hostedServices)
            {
                await hostedService.StartAsync(cancellationToken);
            }
        }
        public async Task StopAllAsync(CancellationToken cancellationToken)
        {
            foreach (var hostedService in _hostedServices)
            {
                await hostedService.StopAsync(cancellationToken);
            }
        }
    }
}
