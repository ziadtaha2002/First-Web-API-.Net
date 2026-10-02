using Azure.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Immutable;
using WebApi.DTO;
using WebApi.Models;
using WebApiDotNet.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        ITIContext context;
        public DepartmentController(ITIContext _Context) 
        {
            context = _Context;
        }




        [HttpGet]
        public IActionResult GetAllDepartments()
        {
            List<Department> deptlist = context.Departments.ToList();
            return Ok(new { deptlist, Message = "Get all departments" });
        }

        // Action to get department and count emps with DTO

        [HttpGet("Count")]
        public IActionResult GetDeptWithCountEmps()
        {
            List<Department> listDept = context.Departments.Include(e=>e.Employees).ToList();

            List<DeptWithEmpsCount> LastlistDTo = new List<DeptWithEmpsCount>();
            foreach(var item in listDept)
            {
                DeptWithEmpsCount ListDptDTO = new DeptWithEmpsCount();
                ListDptDTO.Id = item.Id ;
                ListDptDTO.Name = item.Name ;
                ListDptDTO.ManagerName = item.ManagerName ;
                ListDptDTO.EmployeesCount = item.Employees.Count;

                LastlistDTo.Add(ListDptDTO);
            }

            return Ok(new{LastlistDTo,Message ="Get All Depts with count of Emps"});
        }

        [HttpGet("GetAllDepartmentsWithEmployees")]
        
        public async Task<IActionResult> GetAllDepartmentsAndEmps()
        {
            var Result = await context.Departments
                .Select(d => new
                {
                    Id = d.Id,
                    Name = d.Name,
                    ManagerName = d.ManagerName,
                    // Select only the employee fields you need to display
                    Employees = d.Employees.Select(e => new
                    {
                        Id = e.Id,
                        Name = e.Name,
                        adress = e.Adress,
                    
                    }).ToList()
                })
                .ToListAsync();

            return Ok(new
            {
                deptList = Result,
                message = "Get all departments"
            });
        }



     [HttpGet("{id:int}")] // 👈 أضفنا المعامل ونوعه هنا لتمييز المسار عن الدالة الأولى
       public IActionResult GetDeptById(int id)
        {

            Department dept = context.Departments.FirstOrDefault(d => d.Id == id);
        
            if (dept == null) return NotFound("Department not found"); // حماية في حال عدم وجود القسم

            return Ok(dept);
        }

     [HttpGet("{Name:alpha}")] 
      public IActionResult GetDeptByName(string Name)
        {
            Department dept = context.Departments.FirstOrDefault(d => d.Name == Name);
        
            if (dept == null) return NotFound("Department not found"); // حماية في حال عدم وجود القسم
            return Ok(dept);
        }

        [HttpPost]
      public IActionResult AddDept(Department dept)
        {

            context.Departments.Add(dept);
            context.SaveChanges();
            return CreatedAtAction(nameof(GetDeptById), new { id = dept.Id }, dept);
        }

        [HttpPut]
        public IActionResult UpdateDept(int id, Department dept)
        {
            if (id != dept.Id) return BadRequest("Department ID mismatch");

            var existingDept = context.Departments.FirstOrDefault(d => d.Id == id);
            if (existingDept == null) return NotFound("Department not found");

            existingDept.Name = dept.Name;
            existingDept.ManagerName = dept.ManagerName;
            context.SaveChanges();

            return NoContent();
        }



        [HttpDelete]
        public IActionResult RemoveDept(int id)
        {
            var dept = context.Departments.FirstOrDefault(d => d.Id == id);

            if (dept == null) 
            { 
                return NotFound();
            }
            context.Remove(dept);
            context.SaveChanges();
            return Ok(new { Message = "delete success" });
        }



    }

}
