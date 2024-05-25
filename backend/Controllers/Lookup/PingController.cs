using Microsoft.AspNetCore.Mvc;

namespace backend;

/// <summary>
/// Returns "The service is up and running."
/// </summary>
[ApiController]
[Route("[controller]")]
[ApiExplorerSettings(GroupName = "v1")]
public class PingController : ControllerBase
{
    /// <summary>
    /// Returns "The service is up and running."
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Ping()
    {
        return Ok("The service is up and running.");
    }
}