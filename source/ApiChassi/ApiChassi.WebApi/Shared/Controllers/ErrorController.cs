using Microsoft.AspNetCore.Mvc;

namespace ApiChassi.WebApi.Shared.Controllers
{
    [ApiController]
    public class ErrorController: ControllerBase
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("/error")]
        public IActionResult Error() => Problem();
    }
}
