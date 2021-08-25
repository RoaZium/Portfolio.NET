using System;

namespace PrismWPF.Common.Infrastructure
{
    public static class DateTimeUtils
    {
        public static long GetCurrentTimeStampMilis()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }
    }
}
