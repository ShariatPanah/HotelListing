using System;

namespace HotelListing.Common
{
    public static class EnumExtensions
    {
        public static int ToInt<T>(this T value) where T : IConvertible //enum
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enum type.");

            return (int)(IConvertible)value;
        }

        public static int Count<T>(this T value) where T : IConvertible //enum
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");

            return Enum.GetNames(typeof(T)).Length;
        }
    }
}
