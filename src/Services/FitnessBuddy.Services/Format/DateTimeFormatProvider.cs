namespace FitnessBuddy.Services.Format
{
    using System;

    public class DateTimeFormatProvider : IDateTimeFormatProvider
    {
        public string GetDateFormat(DateTime dateTime)
         => dateTime.ToString("MM/dd/yyyy HH:mm");
    }
}
