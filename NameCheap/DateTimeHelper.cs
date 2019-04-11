using System;
using System.Globalization;

namespace NameCheap
{
    internal static class DateTimeHelper
    {
        internal static DateTime ParseNameCheapDate(this string dateTimeString)
        {
            return DateTime.ParseExact(dateTimeString, "d", CultureInfo.InvariantCulture);
        }
    }
}
