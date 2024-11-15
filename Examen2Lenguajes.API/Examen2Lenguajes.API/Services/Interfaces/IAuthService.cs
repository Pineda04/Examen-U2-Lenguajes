using Examen2Lenguajes.API.Dtos.Auth;
using Examen2Lenguajes.API.Dtos.Common;

namespace Examen2Lenguajes.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ResponseDto<LoginResponseDto>> LoginAsync(LoginDto dto);

        Task<ResponseDto<LoginResponseDto>> RegisterAsync(RegisterDto dto);
    }
}