namespace AK
{
    public partial class App : Application
    {
        private readonly HostedServiceExecutor _hostedServiceExecutor;
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _hostedServiceExecutor = serviceProvider.GetRequiredService<HostedServiceExecutor>();
            MainPage = new MainPage();
        }
        protected override async void OnStart()
        {
            var cancellationToken = new CancellationTokenSource().Token;
            await _hostedServiceExecutor.StartAllAsync(cancellationToken);
        }
        protected override async void OnSleep()
        {
            var cancellationToken = new CancellationTokenSource().Token;
            await _hostedServiceExecutor.StopAllAsync(cancellationToken);
        }
        protected override async void OnResume()
        {
            var cancellationToken = new CancellationTokenSource().Token;
            await _hostedServiceExecutor.StartAllAsync(cancellationToken);
        }
    }
}
