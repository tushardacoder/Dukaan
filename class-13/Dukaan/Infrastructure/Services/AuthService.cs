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
    private readonly IConfiguration _configuration;
   

    public AuthService(MerchantRepository merchantRepository, IConfiguration configuration)
    {
        _merchantRepository = merchantRepository;
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

        var expiration = DateTime.UtcNow.AddHours(1);


        var token = GenerateJwtToken(user,expiration);

        return new AuthResponseDTO(
                 token,
                 expiration


         );



    }


    private string GenerateJwtToken(Merchant user,DateTime expiration)
    {
        var claims = new List<Claim>
        {
            new Claim("sub", user.Id.ToString()),
            new Claim("email", user.Email ??" "),
            new Claim("tenant_id", user.TenantId.ToString())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }


}