using System.ComponentModel.DataAnnotations;

namespace STech.Presentation.Api.Dtos.Account
{
    public class RefreshTokenDto
    {
        [Required(ErrorMessage = "RefreshToken is required")]
        public string RefreshToken { get; set; }
    }
}
