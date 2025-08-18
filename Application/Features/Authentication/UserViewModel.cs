using System.ComponentModel.DataAnnotations;

namespace Application.Features.Authentication
{
    public class UserViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
