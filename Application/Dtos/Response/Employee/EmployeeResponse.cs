namespace Application.Dtos.Response.Employee
{
    /// <summary>
    /// Response خفيف لعرض قائمة الموظفين (GetAll / Search)
    /// </summary>
    public class EmployeeResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string CivilId { get; set; } = string.Empty;
        public string FileNumber { get; set; } = string.Empty;
        public string Fullname { get; set; } = string.Empty;
        public string JobName { get; set; } = string.Empty;
        public string Photo { get; set; } = string.Empty;

        public string? BranchName { get; set; }
        public string? DepartmentName { get; set; }
        public string? GeneralDepartmentName { get; set; }
    }
}
