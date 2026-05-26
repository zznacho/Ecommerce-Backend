using System.Text.RegularExpressions;
using MiApp.Application.Interfaces;
using MiApp.Domain.Entities;

namespace MiApp.Application.UseCases;

public class RegisterUseCase
{
    private readonly IUserRepository _userRepo;

    public RegisterUseCase(IUserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    public async Task<User> Execute(string email, string name, string password)
    {
        // Validaciones básicas
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("El email es obligatorio.");
        var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        if (!Regex.IsMatch(email, emailPattern)) throw new ArgumentException("El email no tiene un formato válido.");

        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("El nombre es obligatorio.");

        if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("La contraseña es obligatoria.");
        if (password.Length < 8) throw new ArgumentException("La contraseña debe tener al menos 8 caracteres.");
        if (!Regex.IsMatch(password, "[A-Z]")) throw new ArgumentException("La contraseña debe contener al menos una letra mayúscula.");
        if (!Regex.IsMatch(password, "[a-z]")) throw new ArgumentException("La contraseña debe contener al menos una letra minúscula.");
        if (!Regex.IsMatch(password, "[0-9]")) throw new ArgumentException("La contraseña debe contener al menos un número.");
        if (!Regex.IsMatch(password, "[^a-zA-Z0-9]")) throw new ArgumentException("La contraseña debe contener al menos un carácter especial.");

        var existing = await _userRepo.GetByEmailAsync(email);
        if (existing is not null) throw new InvalidOperationException("El email ya está registrado.");

        var hash = BCrypt.Net.BCrypt.HashPassword(password);
        var user = new User(email, name, hash);
        await _userRepo.AddAsync(user);
        return user;
    }
}
