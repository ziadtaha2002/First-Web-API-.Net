namespace WebApi.DTO
{
    public class DeptWithEmpsCount
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? ManagerName { get; set; }
        public int EmployeesCount { get; set; }
    }

}
