using System;

namespace Producers.Data
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
