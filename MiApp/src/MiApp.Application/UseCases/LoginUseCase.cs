using MiApp.Application.Interfaces;
using MiApp.Domain.Entities;

namespace MiApp.Application.UseCases;

public class LoginUseCase
{
    private readonly IUserRepository _userRepo;
    private readonly ITokenService _tokenService;

    public LoginUseCase(IUserRepository userRepo, ITokenService tokenService)
    {
        _userRepo = userRepo;
        _tokenService = tokenService;
    }

    public async Task<string?> Execute(string email, string password)
    {
        var user = await _userRepo.GetByEmailAsync(email);
        if (user is null) return null;

        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)) return null;

        return _tokenService.GenerateToken(user);
    }
}
