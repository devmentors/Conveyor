using Microsoft.AspNetCore.Mvc;

namespace Conveyor.Services.Orders.Controllers
{
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get() => "Orders Service";
    }
}
