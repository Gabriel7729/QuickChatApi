using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ApiTemplate.Web.Endpoints.HealthCheckEndpoints;

[ApiController]
[Route("api/[controller]")]
public class Health : ControllerBase
{
  [HttpGet("Check")]
  [SwaggerOperation(
      Summary = "Health Check endpoint",
      Description = "Returns OK if the API is up and running",
      OperationId = "Health.Check",
      Tags = new[] { "HealthEndpoints" })
  ]
  public ActionResult<object> HandleAsync(CancellationToken cancellationToken = default)
  {
    var response = new
    {
      Status = "OK",
      Timestamp = DateTime.Now,
      Version = "1.0.0"
    };

    return Ok(response);
  }
}
