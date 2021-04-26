using Beanstalkd.Consumer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Turbocharged.Beanstalk;

namespace Beanstalkd.Consumer
{
    [ApiController]
    public sealed class BeanstalkdConsumerService : BackgroundService
    {
        private const string Connection = "beanstalkd:11300";

        private readonly ILogger<BeanstalkdConsumerService> _logger;
        private IConsumer _consumer;

        public BeanstalkdConsumerService(ILogger<BeanstalkdConsumerService> logger)
        {
            _logger = logger;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _consumer = await BeanstalkConnection.ConnectConsumerAsync(Connection);
            await _consumer.WatchAsync("test_tube");
            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var task = await _consumer.ReserveAsync<ScheduledTask>();
                _logger.LogTrace($"Task id: {task.Object.Id}");
                Console.WriteLine($"Consumed task id: {task.Object.Id}");
                var result = await _consumer.DeleteAsync(task.Id);
            }
        }

        public override void Dispose()
        {
            _consumer?.Dispose();
        }
    }
}
