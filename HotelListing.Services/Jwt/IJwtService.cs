using HotelListing.Core;
using System.Threading.Tasks;

namespace HotelListing.Services.Jwt
{
    public interface IJwtService
    {
        Task<string> Generate(AppUser user);
    }
}