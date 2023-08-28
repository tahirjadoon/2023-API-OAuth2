using Microsoft.AspNetCore.Mvc.Filters;
using OAuth2.WebApi.Core.Data.BusinessLogic;
using OAuth2.WebApi.Core.Extensions;

namespace OAuth2.WebApi.Core.ActionFilters;

public class LogUserActivityFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        //update after the activity so use next 
        //we will get ActionExecutedContext here
        var resultContext = await next();

        var user = resultContext.HttpContext.User;

        //user must be logged in 
        if (!user.Identity.IsAuthenticated)
            return;

        //we can get the individual properties or the full claims object that has every thing 
        var userName = user.GetUserName();
        var guid = user.GetUserGuid();
        var id = user.GetUserId();
        var claims = user.GetUserClaims();
        if (claims == null)
            return;

        //get the reference to the user business logic
        var userBl = resultContext.HttpContext.RequestServices.GetRequiredService<IUserBusinessLogic>();

        //call method to update the last active date 
        await userBl.LogUserActivityAsync(userName);
    }
}