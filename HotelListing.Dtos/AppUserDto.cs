using System.ComponentModel.DataAnnotations;

namespace HotelListing.Dtos
{
    public class AppUserDto : LoginUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        //[DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
    }

    public class LoginUserDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
