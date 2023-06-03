
using System.ComponentModel.DataAnnotations;

namespace STech.Services.Employee.Api.Dtos.Account
{
    public class VerifyTotpDto
    {
        [Required(ErrorMessage = "UserId is required")]
        public long UserId { get; set; }

        [Required(ErrorMessage = "Code is required")]
        public string Code { get; set; }

        //[Required(ErrorMessage = "isDeveloper is required")]
        //public bool isDeveloper { get; set; } = false;
    }
}
