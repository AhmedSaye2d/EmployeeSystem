namespace Application.Dtos.Response
{
    public record GeneralResponse(bool Flag, string Message, object? Data = null);
}
