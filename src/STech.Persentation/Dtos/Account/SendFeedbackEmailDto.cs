
using System.ComponentModel.DataAnnotations;

namespace STech.Presentation.Api.Dtos.Account
{
    public class SendFeedbackEmailDto
    {
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Email address is not valid.")]
        public string EmailAddress { get; set; } 

        [Required(ErrorMessage = "Subject is required")]
        [StringLength(128, ErrorMessage = "The Subject value cannot exceed 128 characters. ")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Body is required")]
        [StringLength(1024, ErrorMessage = "The Body message value cannot exceed 1024 characters. ")]
        public string Body { get; set; }
    }
}
