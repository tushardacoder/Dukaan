using Dukaan.Application.Dtos;
using Dukaan.Infrastructure.Data.Model;
using Dukaan.Infrastructure.Data.Repositories;
using Dukaan.Infrastructure.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthService : IAuthService
{
    private readonly MerchantRepository _merchantRepository;
    private readonly ITokenService _tokenservice;
    private readonly IConfiguration _configuration;


    public AuthService(MerchantRepository merchantRepository, ITokenService tokenService,IConfiguration configuration)
    {
        _merchantRepository = merchantRepository;
        _tokenservice = tokenService;
        _configuration = configuration;
       
        
   
    }

    public async Task<AuthResponseDTO> LoginAsync(LoginRequestDTO request)

    {
        var user = await _merchantRepository.GetByEmailAsync(request.Email);
       

        if (user == null)
            throw new UnauthorizedAccessException("Invalid Email credentials");

        var isValid = await _merchantRepository.GetByPasswordAsync(user, request.Password);

        if (!isValid)
            throw new UnauthorizedAccessException("Invalid Password credentials");

        var expiration = DateTime.UtcNow.AddMinutes(
                 int.Parse(_configuration["Jwtexpire:ExpiryMinutes"])
       );

        var tokenDto = TokenDtoMapper.MapToTokenDto(user);

        var token = _tokenservice.GenerateToken(tokenDto,expiration);

        return new AuthResponseDTO(
                 token,
                 expiration


         );



    }


   

}