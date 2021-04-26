using Beanstalkd.Consumer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Redis.Consumer
{
    [ApiController]
    public sealed class RedisConsumerService : BackgroundService
    {        
        private readonly ILogger<RedisConsumerService> _logger;
        private ConnectionMultiplexer _redisDb;

        public RedisConsumerService(ILogger<RedisConsumerService> logger)
        {           
            _logger = logger;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _redisDb = await ConnectionMultiplexer.ConnectAsync("redis");
            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ISubscriber subRedisRdb = _redisDb.GetSubscriber();
            await subRedisRdb.SubscribeAsync("test_tube",
                (channel, value) =>
                {
                    var task = JsonConvert.DeserializeObject<ScheduledTask>(value);
                    _logger.LogTrace($"Task id: {task.Id}");
                    Console.WriteLine($"Consumed task id: {task.Id}");
                });
        }

        public override void Dispose()
        {
            _redisDb?.Dispose();
        }
    }
}
