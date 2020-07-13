using Microsoft.AspNetCore.Mvc;

namespace WindowsServiceCore.Controllers
{
	[Route("api/[controller]")]
	public class HelloController : ControllerBase
	{
		[HttpGet]
		public ActionResult GetHello()
		{
			return Ok("Hello!!");
		}
	}
}
