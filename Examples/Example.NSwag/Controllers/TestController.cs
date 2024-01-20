using FunCode.QueryRoute;
using Microsoft.AspNetCore.Mvc;

namespace Example.NSwag.Controllers
{
    [ApiController]
    [Route("tests")]
    public class TestController : ControllerBase
    {
        [HttpGet("")]
        public string DownloadAction([QueryValue("download")] string action)
        {
            return $"action={action}";
        }

        [HttpGet("")]
        public string PingAction([QueryValue("ping")] string action)
        {
            return $"action={action}";
        }
    }
}
