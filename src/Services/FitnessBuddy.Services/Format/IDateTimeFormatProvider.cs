namespace FitnessBuddy.Services.Format
{
    using System;

    public interface IDateTimeFormatProvider
    {
        public string GetDateFormat(DateTime dateTime);
    }
}
