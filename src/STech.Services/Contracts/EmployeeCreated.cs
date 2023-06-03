using STech.Services.Employee.Api.Common;
using System;

namespace STech.Services.Employee.Api.Contracts
{
    public interface EmployeeCreated
    {
        public Meta Meta { get; set; }
        public Data Data { get; set; }

        //public Guid Id { get; set; }
        //public DateTimeOffset CreatedAt { get;}
        //public byte State { get;} 
        //public object Data { get;}
        //public Guid Employee.ApiId { get; }

        //public string Employee.ApiAddress { get; set; }
        //public long UserId { get; set; }

    }


    public class Data
    {
  
        public Guid eventId { get; set; }
        public EmployeeCreationConfirmedData result { get; set; }
        public DateTime createdAt { get; set; }
    }

    public class EmployeeCreationConfirmedData
    {
        public string EmployeeAddress { get; set; }
        public long Id { get; set; }

    }

   

    
}
