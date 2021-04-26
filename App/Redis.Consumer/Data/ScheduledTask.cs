using System;

namespace Beanstalkd.Consumer.Data
{
    public sealed class ScheduledTask
    {
        public ScheduledTask(Guid id, string uniqueCode)
        {
            Id = id;
            UniqueCode = uniqueCode;
        }

        public Guid Id { get; }

        public string UniqueCode { get; }
    }
}
