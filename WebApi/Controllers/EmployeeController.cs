using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using WebApi.Models;
using WebApiDotNet.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        ITIContext context;
        public EmployeeController(ITIContext _Context)
        {
            context = _Context;
        }

        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            //bring all employees with their department name
            var result = context.Employees.Select(e => new
            {
                id = e.Id,
                name = e.Name,
                adress = e.Adress,
                Dept = e.Department.Name

            }).ToList();

            return Ok(new
            {
                deptList = result,
                message = "Get all departments"
            });
        }



    }



}
