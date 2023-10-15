using System.Net;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("v{version:apiVersion}/[controller]")]
public class ExampleController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ExampleResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> FetchAsync([FromQuery] QueryExampleRequest request) { }

    [HttpPost]
    [ProducesResponseType(typeof(ExampleResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> PostAsync([FromBody] CreateExampleRequest request) { }

    [HttpGet("/{id}")]
    [ProducesResponseType(typeof(ExampleResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetAsync(Guid id) { }

    [HttpPut("/{id}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> PutAsync(Guid id, [FromBody] UpdateExampleRequest request) { }

    [HttpDelete("/{id}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> DeleteAsync(Guid id) { }
}