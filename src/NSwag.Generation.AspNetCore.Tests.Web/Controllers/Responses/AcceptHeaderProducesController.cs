using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NSwag.Generation.AspNetCore.Tests.Web.Controllers.Responses
{
    [ApiController]
    [Route("api/[controller]")]
    public class AcceptHeaderProducesController : Controller
    {
        [HttpGet("ProducesOnOperation")]
        [Produces("application/json", "application/type1+json")]
        [ProducesResponseType(typeof(MyResponse), StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType(typeof(MyResponseType1), StatusCodes.Status200OK, "application/type1+json")]
        public IActionResult ProducesOnOperation()
        {
            var acceptHeader = Request.Headers.Accept.ToString();
            if (acceptHeader.Contains("application/type1+json"))
            {
                return Ok(new MyResponseType1());
            }
            return Ok(new MyResponse());
        }
    }

    public class MyResponse
    {
        public string MyProperty { get; set; }
    }

    public class MyResponseType1
    {
        public string ADifferentProperty { get; set; }
    }
}