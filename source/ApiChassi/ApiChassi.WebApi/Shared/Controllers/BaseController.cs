using Microsoft.AspNetCore.Mvc;

namespace ApiChassi.WebApi.Shared.Controllers
{
    /// <summary>
    /// A basic setup Api Controller
    /// </summary>
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    public class BaseController : ControllerBase { }
}
