using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace HotelListing.Core
{
    public class AppUser : IdentityUser
    {
        public AppUser()
        {
            SecurityStamp = Guid.NewGuid().ToString();
        }

        [Required]
        [StringLength(maximumLength: 50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string LastName { get; set; }
    }
}
