namespace NewsWebsite.Controllers;

[AllowAnonymous]
public class HomeController : Controller
{
	public IActionResult Index()
	{
        return View();
	}

	public IActionResult Pricing()
	{
		return View();
	}

	public IActionResult AboutMe()
	{
		return View();
	}
}