using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApiDotNet.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        ITIContext context;
        public ProductController(ITIContext _Context) 
        {
            context = _Context;
        }

        [HttpGet]
        public IActionResult GetAllProduct()
        {
            List<Product> prodcts = context.Products.ToList();

            return Ok(new { prodcts , Message = "Get all departments" });
        }

        [HttpPost]
        public IActionResult AddProduct( Product item)
        {
            context.Products.Add(item);
            context.SaveChanges();
            return CreatedAtAction(nameof(GetAllProduct), new { id = item.Id }, item);
        } 
    }
}
