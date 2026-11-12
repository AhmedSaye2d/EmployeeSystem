namespace Domain.Entities
{
    public class GeneralDepartment : BaseEntity
    {
        public List<Department> Departments { get; set; } = new List<Department>();
    }
}
