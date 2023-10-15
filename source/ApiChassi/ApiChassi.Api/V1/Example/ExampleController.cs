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
    public async Task<IActionResult> FetchAsync([FromQuery] QueryExampleRequest request)
    {
        var data = await Task.FromResult(new[] { new Example() }.ToList());
        Response?.Headers?.Add("X-Total-Count", data.Count.ToString());
        return Ok(data);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ExampleResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> PostAsync([FromBody] CreateExampleRequest request)
    {
        var result = await Task.FromResult(request);
        return CreatedAtAction(nameof(GetAsync), new { id = Guid.NewGuid() }, result);
    }

    [HttpGet("/{id}")]
    [ProducesResponseType(typeof(ExampleResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetAsync(Guid id)
    {
        var data = await Task.FromResult(new Example { Id = id });
        return Ok(data);
    }

    [HttpPut("/{id}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> PutAsync(Guid id, [FromBody] UpdateExampleRequest request)
    {
        return NoContent();
    }

    [HttpDelete("/{id}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        return NoContent();
    }
}