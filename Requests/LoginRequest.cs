using Movies.WebAPI.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace Movies.WebAPI.Requests
{
    public class LoginRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        //[PhoneNumber]
        //[RegularExpression("^[0-9]{3}-[0-9]{3}-[0-9]{4}$", ErrorMessage ="Must be of format xxx-xxx-xxxx")]
        //public string PhoneNumber { get; set; }

    }
}
