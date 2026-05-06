using Dukaan.Application.Dtos;

namespace Dukaan.Infrastructure.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> LoginAsync(LoginRequestDTO request);
    }

}
