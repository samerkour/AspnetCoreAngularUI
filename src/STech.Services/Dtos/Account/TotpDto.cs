
using System.ComponentModel.DataAnnotations;

namespace STech.Services.Employee.Api.Dtos.Account
{
    public class TotpDto
    {
        [Required(ErrorMessage = "Phone Number is required")]
        //[Range(1, ulong.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public ulong? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Country Code is required")]
        //[Range(1, ushort.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public ushort? CountryCode { get; set; } = 98;
    }
}
