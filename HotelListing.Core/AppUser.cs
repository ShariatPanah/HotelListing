using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HotelListing.Core
{
    public class AppUser : IdentityUser
    {
        [Required]
        [StringLength(maximumLength: 50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string LastName { get; set; }
    }
}
