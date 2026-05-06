using Dukaan.Application.Dtos;
using Dukaan.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dukaan.Host.Controllers
{

    [ApiController]
    [Route("api/[controller]")] // domain/api/auth
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
               _authService = authService;
        }

        [HttpPost("login")] // domain/api/auth/login


        public async Task<ActionResult> Login(LoginRequestDTO loginrequestdto)
        {
                 var  response=_authService.LoginAsync(loginrequestdto);
                 return  Ok(response);
        }

    }
}
