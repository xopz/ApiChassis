namespace ApiChassi.WebApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Route("api/[controller]")]
    public abstract class BaseController : ControllerBase { }
}
