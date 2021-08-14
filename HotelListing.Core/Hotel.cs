using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelListing.Data
{
    public class Hotel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "نام هتل را وارد کنید.")]
        [StringLength(maximumLength: 50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "آدرس هتل را وارد کنید.")]
        [StringLength(maximumLength: 255)]
        public string Address { get; set; }

        [Range(minimum: 0, maximum: 10)]
        public double Rating { get; set; }

        [ForeignKey(nameof(Country))]
        public int CountryId { get; set; }

        public Country Country { get; set; }
    }
}
