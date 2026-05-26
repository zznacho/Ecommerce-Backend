using Microsoft.AspNetCore.Mvc;
using MiApp.Application.UseCases;
using MiApp.Application.Interfaces;
using MiApp.Domain.Entities;

namespace MiApp.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly LoginUseCase _loginUseCase;
    private readonly RegisterUseCase _registerUseCase;

    public AuthController(LoginUseCase loginUseCase, RegisterUseCase registerUseCase)
    {
        _loginUseCase = loginUseCase;
        _registerUseCase = registerUseCase;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var token = await _loginUseCase.Execute(request.Email, request.Password);
        if (token is null) return Unauthorized(new { message = "Credenciales incorrectas" });
        return Ok(new { token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var user = await _registerUseCase.Execute(request.Email, request.Name, request.Password);
            return CreatedAtAction(nameof(Register), new { id = user.Id }, new { user.Id, user.Email, user.Name });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}

public record LoginRequest(string Email, string Password);
public record RegisterRequest(string Email, string Name, string Password);
