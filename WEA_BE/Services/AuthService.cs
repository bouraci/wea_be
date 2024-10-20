using AutoMapper;
using EFModels.Data;
using EFModels.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using WEA_BE.DTO;

namespace WEA_BE.Services
{
    public class AuthService : IAuthService
    {
        private readonly DatabaseContext _ctx;
        private readonly IMapper _mapper;

        public AuthService(DatabaseContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<bool> RegisterAsync(string name, string username, string password)
        {
            if (await _ctx.Set<User>().AnyAsync(u => u.UserName == username))
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

            _ctx.Set<User>().Add(user);
            await _ctx.SaveChangesAsync();

            return true;
        }


        private static string HashPassword(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(20);
                return Convert.ToBase64String(hash);
            }
        }
        public async Task<UserDto?> LoginAsync(string username, string password)
        {
            var user = await _ctx.Set<User>().FirstOrDefaultAsync(u => u.UserName == username);
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
                return _mapper.Map<UserDto>(user);
            }
            return null;

        }
    }
}

