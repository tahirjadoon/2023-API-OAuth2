namespace OAuth2.WebApi.Core.Extensions;

public static class DateTimeExtensions
{
    /// <summary>
    /// Calculate Age
    /// </summary>
    /// <param name="dob"></param>
    /// <returns>Age</returns>
    public static int CalculateAge(this DateOnly dob)
    {
        //for more accurate calculation 
        //https://stackoverflow.com/questions/3054715/c-sharp-calculate-accurate-age

        //todays date
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        //calcuate the age
        var age = today.Year - dob.Year;

        //go back to the year in which the person was born in case of a leap year
        if (dob > today.AddYears(-age))
            age--;

        return age;
    }
}
