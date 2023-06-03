using System;
using System.Collections.Generic;

namespace STech.Domain.Entities.EmployeeEntities
{
    public partial class Employee
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string AccountNumber { get; set; }
    }
}
