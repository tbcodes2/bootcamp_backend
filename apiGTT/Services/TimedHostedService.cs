using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

    internal class TimedHostedService : IHostedService, IDisposable

    {
        private readonly ILogger _logger;
        private Timer _timer;

        public TimedHostedService (ILogger <TimedHostedService> logger)
        {
            _logger = logger;
        }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Timed Background Service is starting.");

        _timer = new Timer(DoWork, null, TimeSpan.Zero,
            TimeSpan.FromHours(12));

        return Task.CompletedTask;
    }

    public void DoWork(object state)
        {
            _logger.LogInformation("Ejecutando tarea");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Parando el servicio");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

    }
