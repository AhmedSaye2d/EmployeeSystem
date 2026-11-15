namespace Application.Dtos.Response.Department
{
    public class DepartmentResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int GeneralDepartmentId { get; set; }
        public string? GeneralDepartmentName { get; set; }
    }
}
