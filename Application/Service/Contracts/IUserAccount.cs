using Application.Dtos.AccountDto;
using Application.Dtos.Response;

namespace Application.Service.Contracts
{
    public interface IUserAccount
    {
        Task<GeneralResponse> CreateAsync(Register register);
        Task<LoginResponse> LoginAsync(Login login);
        Task<LoginResponse> RefreshTokenAsync(RefreshToken token);
    }
}

namespace Infrastructure.Contracts
{
    // Alias to prevent namespace mismatch errors if Infrastructure.Contracts is used
    public interface IUserAccount : Application.Service.Contracts.IUserAccount { }
}

namespace Application.Contracts
{
    // Alias to prevent namespace mismatch errors if Application.Contracts is used
    public interface IUserAccount : Application.Service.Contracts.IUserAccount { }
}
