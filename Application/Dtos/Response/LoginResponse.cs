namespace Application.Dtos.Response
{
    public record LoginResponse(bool Flag, string Message, string Token = null!, string RefreshToken = null!);
}