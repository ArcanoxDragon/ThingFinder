using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ThingFinder.Models;

namespace ThingFinder.Controllers;

public class HomeController : Controller
{
	public IActionResult Index() => View();

	public IActionResult Privacy() => View();

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

		return View(new ErrorViewModel { RequestId = requestId });
	}
}