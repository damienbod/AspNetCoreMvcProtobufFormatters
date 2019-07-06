using AspNetCoreProtobuf.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreProtobuf.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values/5
        [HttpGet("{id}")]
        public ProtobufModelDto Get(int id)
        {
            return new ProtobufModelDto() { Id = 1, Name = "HelloWorld", StringValue = "My first MVC 6 Protobuf service" };
        }

        [HttpPost]
        [Route("")]
        public IActionResult Post([FromBody]ProtobufModelDto value)
        {
            // Yes the value can be sent as a protobuf item.
            var myValue = value;
            if(value == null)
            {
                return BadRequest("no data");
            }

            return NoContent();
        }

    }
}
