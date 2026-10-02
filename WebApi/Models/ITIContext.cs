using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApiDotNet.Models
{
    public class ITIContext:DbContext
    {
        public DbSet<Department> Departments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public ITIContext(DbContextOptions<ITIContext> options):base(options)
        {

        }
    }
}