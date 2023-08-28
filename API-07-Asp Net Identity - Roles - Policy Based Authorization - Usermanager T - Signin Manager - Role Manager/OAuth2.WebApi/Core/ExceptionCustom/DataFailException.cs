namespace OAuth2.WebApi.Core.ExceptionCustom;

public class DataFailException : Exception
{
    public DataFailException()
    {
    }

    public DataFailException(string message) : base(message)
    {
    }

    public DataFailException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
