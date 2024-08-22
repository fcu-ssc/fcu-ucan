using Microsoft.AspNetCore.Mvc;

namespace fcu_ucan.Controllers;

[Route("")]
public class HomeController : Controller
{
    [HttpGet("")]
    public IActionResult Index() => View();
}