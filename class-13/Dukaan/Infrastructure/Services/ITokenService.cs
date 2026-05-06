using Dukaan.Application.Dtos;

namespace Dukaan.Infrastructure.Services
{
    public interface ITokenService
    {
        string GenerateToken(TokenUserDto user,DateTime expiration);
    }
}
