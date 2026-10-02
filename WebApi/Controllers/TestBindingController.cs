using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestBindingController : ControllerBase
    {
        //[HttpGet] //Url is : https://localhost:44388/api/TestBinding?name=Ahmed&age=20
        [HttpGet("{name:alpha}/{age:int}")] //url is   'http://localhost:58030/api/TestBinding/saa/21' \

        public IActionResult Get( string name,int age)
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult TestObj(Department department , string name)
        {
            return Ok();    
        }
    }
}
