using Microsoft.AspNetCore.Mvc;
using OAuth2.WebApi.Core.ActionFilters;

namespace OAuth2.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[ServiceFilter(typeof(LogUserActivityFilter))]
public class BaseApiController : ControllerBase
{

}
