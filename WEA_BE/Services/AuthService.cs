﻿using AutoMapper;
using EFModels.Data;
using EFModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WEA_BE.DTO;
using WEA_BE.Models;

namespace WEA_BE.Services;

/// <summary>
/// Služba pro správu autentizace uživatelů, zahrnující registraci a přihlášení.
/// </summary>
public class AuthService : IAuthService
{
    private readonly DatabaseContext _ctx;
    private readonly IMapper _mapper;
    private readonly JwtSecretKey _secretKey;

    /// <summary>
    /// Konstruktor služby autentizace, který přijímá kontext databáze a mapper pro mapování objektů.
    /// </summary>
    /// <param name="ctx">Kontext databáze pro přístup k uživatelům.</param>
    /// <param name="mapper">Automapper pro mapování mezi entitami a DTO.</param>
    public AuthService(DatabaseContext ctx, IMapper mapper, JwtSecretKey secretKey)
    {
        _ctx = ctx;
        _mapper = mapper;
        _secretKey = secretKey;
    }

    /// <summary>
    /// Zaregistruje nového uživatele s poskytnutými údaji.
    /// </summary>
    /// <param name="name">Jméno uživatele.</param>
    /// <param name="username">Uživatelské jméno.</param>
    /// <param name="password">Heslo uživatele.</param>
    /// <returns>Vrací true, pokud registrace proběhla úspěšně, nebo false, pokud uživatel s daným uživatelským jménem již existuje.</returns>
    public async Task<bool> RegisterAsync(string name, string username, string password)
    {
        if (await _ctx.Users.AnyAsync(u => u.UserName == username))
        {
            return false;
        }



        byte[] salt = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        string passwordHash = HashPassword(password, salt);

        var user = new User
        {
            Name = name,
            UserName = username,
            PasswordHash = Convert.ToBase64String(salt) + ":" + passwordHash
        };

        _ctx.Users.Add(user);
        await _ctx.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Zajistí hashování hesla pomocí algoritmu PBKDF2.
    /// </summary>
    /// <param name="password">Heslo k hashování.</param>
    /// <param name="salt">Salt pro hashování hesla.</param>
    /// <returns>Vrací hash hesla ve formě Base64 řetězce.</returns>
    private static string HashPassword(string password, byte[] salt)
    {
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256))
        {
            byte[] hash = pbkdf2.GetBytes(20);
            return Convert.ToBase64String(hash);
        }
    }

    /// <summary>
    /// Přihlašuje uživatele na základě uživatelského jména a hesla.
    /// </summary>
    /// <param name="username">Uživatelské jméno.</param>
    /// <param name="password">Heslo uživatele.</param>
    /// <returns>Vrací DTO uživatele, pokud je přihlášení úspěšné, nebo null, pokud je přihlášení neúspěšné.</returns>
    public async Task<string?> LoginAsync(string username, string password)
    {
        var user = await _ctx.Users.FirstOrDefaultAsync(u => u.UserName == username);
        if (user == null)
        {
            return null;
        }

        var parts = user.PasswordHash.Split(':');
        var salt = Convert.FromBase64String(parts[0]);
        var storedHash = parts[1];

        string enteredPasswordHash = HashPassword(password, salt);

        if (storedHash == enteredPasswordHash)
        {
            string token = GenerateJwtToken(user);
            return token;
        }
        return null;

    }

    public UserDto? Authorize(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var expirationClaim = jwtToken?.Claims?.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;
        if (expirationClaim != null)
        {
            var expirationTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expirationClaim));
            if (expirationTime < DateTimeOffset.UtcNow)
            {
                return null;
            }
        }
        var userName = jwtToken?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (userName == null) return null;

        var user = _ctx.Users.FirstOrDefault(x => x.UserName == userName);
        return _mapper.Map<UserDto?>(user);
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.Name, user.Name)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "Bouraci",
            audience: "Gooners",
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

