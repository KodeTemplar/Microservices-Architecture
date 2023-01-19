using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
    [Route("api/InC/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        public PlatformsController()
        {

        }

        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("--> Inbound post #Command Service");
            return Ok("Inbound test of From Platform Controller");
        }
    }
}
