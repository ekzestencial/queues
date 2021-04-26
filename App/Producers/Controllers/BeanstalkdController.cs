using Microsoft.AspNetCore.Mvc;
using Producers.Data;
using System;
using System.IO;
using System.Threading.Tasks;
using Turbocharged.Beanstalk;

namespace Producers.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public sealed class BeanstalkdController : ControllerBase
    {
        private const string Connection = "beanstalkd:11300";

        private IProducer _producer;

        public BeanstalkdController()
        {
            _producer = ConnectToQueueAsync().Result;
        }

        [HttpPost]
        [Route("/beanstalkd/send")]
        public async Task PostTaskAsync()
        {
            var task = Create();
            var result = await _producer.PutAsync(task, 1, TimeSpan.Zero);
        }

        private ScheduledTask Create()
        {
            return new ScheduledTask(
                Guid.NewGuid(),
                Path.GetRandomFileName());
        }

        private async Task<IProducer> ConnectToQueueAsync()
        {
            var producer = await BeanstalkConnection.ConnectProducerAsync(Connection);
            await producer.UseAsync("test_tube");
            return producer;
        }
    }
}
