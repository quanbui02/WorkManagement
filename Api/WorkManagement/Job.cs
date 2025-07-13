namespace WorkManagement
{
    public class Job : IHostedService, IDisposable
    {
        private static readonly object Object = new object();
        private readonly ILogger _logger;
        private Timer _timer;
        public DateTime LastExecuted { get; set; }
        public DateTime MergeOrderNeedShipLastExecuted { get; set; } = DateTime.UtcNow;
        public DateTime MergeOrderWattingShipLastExecuted { get; set; } = DateTime.UtcNow;
        public DateTime UserAddressLocationLastExecuted { get; set; } = DateTime.UtcNow;
        public bool runOne = true;
        private readonly IServiceScopeFactory _scopeFactory;

        public Job(
            ILogger<Job> logger,
            IServiceScopeFactory scopeFactory
            )
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(50));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            //lock đối tượng
            lock (Object)
            {
                //System.Diagnostics.Debug.WriteLine("JOB: Start");
                Console.WriteLine("JOB: Start");

                // Thực hiện vào 2 giờ sáng hàng ngày và chỉ thực hiện 1 lần trong ngày
                if (DateTime.UtcNow.Hour == 2 && DateTime.UtcNow.Minute == 0 && DateTime.UtcNow.Date != LastExecuted.Date)
                {
                    LastExecuted = DateTime.UtcNow;
                }

                //// Kích hoạt domain
                //if (Config.Domain.IsJobStart)
                //{
                //    DomainRegister();
                //}
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }


    }
}
