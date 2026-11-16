namespace Application.Dtos.Response.Town
{
    public class TownResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CityId { get; set; }
        public string? CityName { get; set; }
        public int CountryId { get; set; }
        public string? CountryName { get; set; }
    }
}
