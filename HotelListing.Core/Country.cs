using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelListing.Core
{
    public class Country
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "نام کشور را وارد کنید.")]
        [StringLength(maximumLength: 30)]
        public string Name { get; set; }

        [Required(ErrorMessage = "نام خلاصه کشور را وارد کنید.")]
        [StringLength(maximumLength: 10)]
        public string ShortName { get; set; }
    }
}
