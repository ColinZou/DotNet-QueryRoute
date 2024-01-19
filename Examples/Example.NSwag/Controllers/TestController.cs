using FunCode.QueryRoute;
using Microsoft.AspNetCore.Mvc;

namespace Example.NSwag
{
    [ApiController]
    [Route("tests")]
    public class TestController : ControllerBase
    {
        [HttpGet("")]
        public string DownloadAction([RouteParam("download")] string action)
        {
            return $"action={action}\n";
        }

        [HttpGet("")]
        public string PingAction([RouteParam("ping")] string action)
        {
            return $"action={action}\n";
        }
    }
}
