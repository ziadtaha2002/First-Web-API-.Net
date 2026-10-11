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

        #region GetActions
        // Action to get all departments 
        //public IActionResult GetAllDepartments()
        //{
        //    List<Department> deptlist = context.Departments.ToList();
        //    return Ok(new { deptlist, Message = "Get all departments" });

        //}
        [HttpGet]
        public IActionResult GetAllDepartments()
        {
            List<Department> departments = context.Departments.ToList();
            List<DepartmentDto> departmentDtos = departments.
                Select(dept => new DepartmentDto
                {
                    Id = dept.Id,
                    Name = dept.Name
                }).ToList();

            General_Response <List<DepartmentDto>> Response =
                 new General_Response<List<DepartmentDto>>
                 {
                     isSuccess = true,
                     Message = "Get all depts",
                     Data = departmentDtos
                 };
            return Ok(Response);
        }

        // Action to get all departments with count of employees in each department without using DTOs
        // DTOs are used to shape the data and avoid exposing unnecessary details of the entities. In this case, 
        // we want to return the department details along with the count of employees, which is not a direct property of the Department entity.
        [HttpGet("Count")]
        public IActionResult GetDeptWithCountEmps()
        {
            List<Department> listDept = context.Departments.Include(e => e.Employees).ToList();

            List<DeptWithEmpsCount> LastlistDTo = new List<DeptWithEmpsCount>();
            foreach (var item in listDept)
            {
                DeptWithEmpsCount ListDptDTO = new DeptWithEmpsCount();
                ListDptDTO.Id = item.Id;
                ListDptDTO.Name = item.Name;
                ListDptDTO.ManagerName = item.ManagerName;
                ListDptDTO.EmployeesCount = item.Employees.Count;

                LastlistDTo.Add(ListDptDTO);
            }

            General_Response<List<DeptWithEmpsCount>> _Response =
            new General_Response<List<DeptWithEmpsCount>>
            {
                isSuccess = true ,
                Data = LastlistDTo,
                Message = "All depts with count of emps"
            };
          

            return Ok(_Response);
        }

        //Action to get all departments with their employees without using DTOs,
        //but shaping the data in the response to include only necessary fields.
        
        
        //[HttpGet("GetAllDepartmentsWithEmployees")]
        //public async Task<IActionResult> GetAllDepartmentsAndEmps()
        //{
        //    var Result = await context.Departments
        //        .Select(d => new
        //        {
        //            Id = d.Id,
        //            Name = d.Name,
        //            ManagerName = d.ManagerName,
        //            // Select only the employee fields you need to display
        //            Employees = d.Employees.Select(e => new
        //            {
        //                Id = e.Id,
        //                Name = e.Name,
        //                adress = e.Adress,

        //            }).ToList()
        //        })
        //        .ToListAsync();

        //    return Ok(new
        //    {
        //        Result,
        //        message = "Get all departments"
        //    });
        //}
        [HttpGet("get depts and if it have emps")]
        public async Task<IActionResult> GetDeptsWithEmps()
        {
            var result = await context.Departments.Select(d => new
            {
                Id = d.Id,
                Name = d.Name,
                Manager = d.ManagerName,
                Employees = d.Employees.Select(e => new
                {
                    name = e.Name,
                    add = e.Adress,
                    dept = e.Department
                }).ToList()
            }).ToListAsync();

           

            return Ok(result);
        
        }

        //Action to get a department by its ID
        [HttpGet("{id:int}")] // 👈 أضفنا المعامل ونوعه هنا لتمييز المسار عن الدالة الأولى
        public IActionResult GetDeptById(int id)
        {

            Department dept = context.Departments.FirstOrDefault(d => d.Id == id);

            if (dept == null) return NotFound("Department not found"); // حماية في حال عدم وجود القسم

            return Ok(dept);
        }

        //Action to get a department by its Name
        [HttpGet("{Name:alpha}")]
        public IActionResult GetDeptByName(string Name)
        {
            Department dept = context.Departments.FirstOrDefault(d => d.Name == Name);

            if (dept == null) return NotFound("Department not found"); // حماية في حال عدم وجود القسم
            return Ok(dept);
        }

        #endregion 




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
