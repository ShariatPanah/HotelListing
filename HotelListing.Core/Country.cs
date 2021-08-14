using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelListing.Data
{
    public class Country
    {
        public Country()
        {
            Hotels = new List<Hotel>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "نام کشور را وارد کنید.")]
        [StringLength(maximumLength: 30)]
        public string Name { get; set; }

        [Required(ErrorMessage = "نام خلاصه کشور را وارد کنید.")]
        [StringLength(maximumLength: 10)]
        public string ShortName { get; set; }

        //[StringLength(maximumLength: 5)]
        //public string ZipCode { get; set; }

        public virtual IList<Hotel> Hotels { get; set; }
    }
}
