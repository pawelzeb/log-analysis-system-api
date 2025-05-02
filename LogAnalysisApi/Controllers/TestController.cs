using Microsoft.AspNetCore.Mvc;

namespace LogAnalysisApi.Controllers;

[ApiController]
[Route("test")]
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult Ping()
    {
        return Ok(new { message = "API działa poprawnie!" });
    }
}
