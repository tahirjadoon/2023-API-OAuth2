using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using OAuth2.WebApi.Core.Constants;
using OAuth2.WebApi.Core.Dto;
using OAuth2.WebApi.Core.ExceptionCustom;

namespace OAuth2.WebApi.Core.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    /// <summary>
    /// Receives RequestDelegate which is whats next in the middle ware pipeline
    /// </summary>
    /// <param name="next">What is next in the pipeline</param>
    /// <param name="logger">So to log the exception</param>
    /// <param name="environment">The environment development/production</param>
    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    /// <summary>
    /// The required method to invoke the middleware
    /// </summary>
    /// <param name="context">The http context</param>
    /// <returns></returns>
    public async Task Invoke(HttpContext context)
    {
        try
        {
            //pass the context to the next piece of middleware
            await _next(context);
        }
        catch (DataFailException dfe)
        {
            _logger.LogError(dfe, dfe.Message);
            await WriteGeneralError(context, dfe, HttpStatusCode.BadRequest);
        }
        catch (ValidationException vex)
        {
            _logger.LogError(vex, vex.Message);
            await WriteGeneralError(context, vex, HttpStatusCode.BadRequest);
        }
        catch (UnauthorizedAccessException uex)
        {
            _logger.LogError(uex, uex.Message);
            await WriteGeneralError(context, uex, HttpStatusCode.Unauthorized);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            await WriteSpecificError(context, ex, HttpStatusCode.InternalServerError);
        }
    }

    private async Task WriteGeneralError(HttpContext context, Exception ex, HttpStatusCode code)
    {
        //set content type
        context.Response.ContentType = ContentTypeConstants.ApplicationJson;
        //set status code
        context.Response.StatusCode = (int)code;

        //write
        if (_environment.IsDevelopment())
            await context.Response.WriteAsync($"{ex.Message} \r\n {ex.StackTrace?.ToString()}");
        else
            await context.Response.WriteAsync(ex.Message);
    }

    private async Task WriteSpecificError(HttpContext context, Exception ex, HttpStatusCode code)
    {
        //set content type
        context.Response.ContentType = ContentTypeConstants.ApplicationJson;
        //set status code
        context.Response.StatusCode = (int)code;

        //create the response model
        ApiExceptionDto response = null;
        if (_environment.IsDevelopment())
        {
            //development put out the exact message and stack trace
            response = new ApiExceptionDto(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString());
        }
        else
        {
            //production do not put out the exact message and stack trace
            response = new ApiExceptionDto(context.Response.StatusCode, "Internal Server Error");
        }

        //want the json responses to go as camel case 
        var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        //serialize the response
        var json = JsonSerializer.Serialize(response, jsonOptions);

        //write
        await context.Response.WriteAsync(json);
    }
}
