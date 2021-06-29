using Microsoft.AspNetCore.Mvc;

namespace ApiChassi.WebApi.Shared.Controllers
{
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    public class BaseController : ControllerBase { }
}
