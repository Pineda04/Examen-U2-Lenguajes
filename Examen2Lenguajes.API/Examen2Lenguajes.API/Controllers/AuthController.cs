using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Examen2Lenguajes.API.Dtos.Auth;
using Examen2Lenguajes.API.Dtos.Common;
using Examen2Lenguajes.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Examen2Lenguajes.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(
            IAuthService authService
            )
        {
            this._authService = authService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseDto<LoginResponseDto>>> Login(LoginDto dto) 
        {
            var response = await _authService.LoginAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseDto<LoginResponseDto>>> Register(RegisterDto dto)
        {
            var response = await _authService.RegisterAsync(dto);
            return StatusCode(response.StatusCode, response);
        }
    }
}