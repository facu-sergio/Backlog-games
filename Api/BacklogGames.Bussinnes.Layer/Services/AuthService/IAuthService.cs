using BacklogGames.Bussinnes.Layer.DTOs.Auth;

namespace BacklogGames.Bussinnes.Layer.Services.AuthService
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
        Task<(bool success, string? error)> ChangePasswordAsync(int userId, ChangePasswordDto request);
    }
}
