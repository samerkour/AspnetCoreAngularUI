using System;
using System.ComponentModel.DataAnnotations;

namespace STech.Services.Employee.Api.Dtos.Employee.Api
{
    public class EmployeeDto
    {

        //public long Id { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Age is required")]
        public int Age { get; set; }

        [Required(ErrorMessage = "BirthDate is required")]
        public DateTimeOffset BirthDate { get; set; }

        [Required(ErrorMessage = "PhoneNumber is required")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "AccountNumber is required")]
        public string AccountNumber { get; set; }


    }

   
}
