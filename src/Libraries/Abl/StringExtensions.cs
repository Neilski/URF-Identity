using System;

namespace Abl
{
    public static class StringExtensions
    {
        public static Guid ToGuid(this string source)
        {
            Guid result;
            if ((String.IsNullOrWhiteSpace(source)) ||
                (!Guid.TryParse(source, out result)))
            {
                return Guid.Empty;
            }

            return result;
        }
    }
}
