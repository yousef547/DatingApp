using System;

namespace DatingApp_API.Extensions
{
    public static class DateTimeExtesions
    {
        public static int CalculatedAge(this DateTime dob)
        {
            var today = DateTime.Today;
            var age = today.Year - dob.Year ;
            if (dob.Date > today.AddYears(-age)) --age;
            return age;
        }
    }
}
