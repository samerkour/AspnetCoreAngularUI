using System;
using System.Collections.Generic;

namespace STech.Domain.Entities.EmployeeEntities
{
    public partial class EmployeeCreatedEvent
    {
        public long Id { get; set; }
        public DateTimeOffset OccurredOn { get; set; }
        public byte State { get; set; }
        public DateTimeOffset? ProcessedOn { get; set; }
        public string Data { get; set; }
        public Guid EventId { get; set; }
    }
}
