namespace Domain.Entities
{
    public class Country : BaseEntity
    {
        public List<City> Cities { get; set; } = new List<City>();
    }
}
