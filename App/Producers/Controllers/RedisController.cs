using BeetleX.Redis;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Producers.Data;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Producers.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public sealed class RedisController : ControllerBase, IDisposable
    {
        private readonly RedisDB _redisDb;

        public RedisController()
        {
            _redisDb = new RedisDB(0, new JsonFormater());
            _redisDb.Host.AddWriteHost("redis");
        }

        [HttpPost]
        [Route("/redis/send")]
        public async Task PostTaskAsync()
        {
            var task = Create();
            var result = await _redisDb.Publish("test_tube", task);
        }

        [HttpGet]
        [Route("/redis/read")]
        public Task<string> GetTaskAsync()
        {
            var task = Create();
            var json = JsonConvert.SerializeObject(task);
            return Task.FromResult(json);
        }

        public void Dispose()
        {
            _redisDb.Dispose();
        }

        private static ScheduledTask Create()
        {
            return new ScheduledTask(
                Guid.NewGuid(),
                Path.GetRandomFileName());
        }
    }
}
