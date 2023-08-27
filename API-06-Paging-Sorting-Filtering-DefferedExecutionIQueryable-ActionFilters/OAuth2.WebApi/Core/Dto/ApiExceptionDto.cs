namespace OAuth2.WebApi.Core.Dto;

/// <summary>
/// Dto used by the ExceptionMiddleware to pass the response back
/// </summary>
public class ApiExceptionDto
{
    public ApiExceptionDto(int statusCode, string message = null, string details = null)
    {
        StatusCode = statusCode;
        Message = message;
        Details = details;
    }

    /// <summary>
    /// Http Status Code
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// Message
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Exception Details
    /// </summary>
    public string Details { get; set; }
}
