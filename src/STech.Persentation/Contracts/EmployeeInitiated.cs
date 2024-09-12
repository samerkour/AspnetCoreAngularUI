using MassTransit;
using System;

namespace STech.Presentation.Api.Contracts
{
    //[EntityName("Employee.ApiCreationEvents")]
    public record EmployeeInitiated
    {
        //public Guid Id { get; init; }
        public DateTimeOffset OccurredOn { get; init; }
        //public byte State { get; init; }
        //public long? UserId { get; set; }
        public EmployeeCreationEventData Data { get; set; }
        public Guid EventId { get; set; }
    }

    public class EmployeeCreationEventData 
    {
        public long UserId { get; set; }
    }
}
