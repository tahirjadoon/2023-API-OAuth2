using OAuth2.WebApi.Core.Constants;

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

    /// <summary>
    /// The oldest the person can be. This is the minimum age. check against MaxAge
    /// </summary>
    /// <param name="age"></param>
    /// <returns></returns>
    public static DateOnly CalculateMinDob(this int age)
    {
        if (age <= 0) age = DataConstants.MaxAge;
        var dob = DateOnly.FromDateTime(DateTime.Today.AddYears(-age));
        return dob;
    }

    /// <summary>
    /// The youngest the person can be. check against min age
    /// </summary>
    /// <param name="age"></param>
    /// <returns></returns>
    public static DateOnly CalculateMaxDob(this int age)
    {
        if (age <= 0) age = DataConstants.MinAge;
        var dob = DateOnly.FromDateTime(DateTime.Today.AddYears(-age - 1));
        return dob;
    }
}
