using Dukaan.Application.Dtos;
using Dukaan.Infrastructure.Data.Model;

namespace Dukaan.Infrastructure.Services
{
    public  class TokenDtoMapper
    {
        public static TokenUserDto MapToTokenDto(Merchant user)
        {
            return new TokenUserDto
            {
                UserId = user.Id,
                Email = user.Email ?? "",
                TenantId = user.TenantId
            };
        }
    }
}
