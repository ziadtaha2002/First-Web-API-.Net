using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;
using WebApi.Models;

namespace WebApiDotNet.Models
{
    public class ITIContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Department> Departments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public ITIContext(DbContextOptions<ITIContext> options):base(options)
        {

        }
    }
}