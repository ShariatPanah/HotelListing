using System.Collections.Generic;

namespace HotelListing.Dtos
{
    public class CreateCountryDto
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
    }

    public class CountryDto : CreateCountryDto
    {
        public CountryDto()
        {
            Hotels = new List<HotelDto>();
        }

        public int Id { get; set; }
        public IList<HotelDto> Hotels { get; set; }
    }
}
