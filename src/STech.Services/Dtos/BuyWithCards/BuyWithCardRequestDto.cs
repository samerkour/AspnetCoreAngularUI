using System;

namespace STech.Services.Employee.Api.Dtos.SendReceive
{
    public class BuyWithCardRequestDto
    {
        //public long Id { get; set; }
        //public long UserId { get; set; }
        public decimal BontiAmount { get; set; }

        //public DateTimeOffset CreateDateTime { get; set; }
        public bool State { get; set; } = false;
        public byte CardPayementMethodId { get; set; }
        public decimal Amount { get; set; }

    }
}
