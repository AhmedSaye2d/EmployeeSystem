namespace Domain.Entities
{
    public class RefreshTokenInfo
    {
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
    }
}
