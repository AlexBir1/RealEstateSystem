namespace DwellingAPI.Extensions
{
    public static class DataTypesExtensions
    {
        public static bool ToBool(this int target)
        {
            return target >= 1;
        }
    }
}