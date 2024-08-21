using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using fcu_ucan.Models;
using fcu_ucan.Models.Error;

namespace fcu_ucan.Controllers;

[Route("")]
public class HomeController : Controller
{
    [HttpGet("")]
    public IActionResult Index() => View();
}