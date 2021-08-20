namespace HotelListing.Common
{
    public class SiteSettings
    {
        public string ElmahPath { get; set; }
        public JwtSettings JwtSettings { get; set; }
    }

    public class JwtSettings
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int NotBeforeInMinutes { get; set; }
        public int NotBeforeInHours { get; set; }
        public int ExpirationInDays { get; set; }
    }
}
