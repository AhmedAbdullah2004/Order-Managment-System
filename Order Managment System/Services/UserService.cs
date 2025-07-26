using System.Security.Cryptography;
using System.Text;
using OrderManagementSystem.DTOs;
using OrderManagementSystem.Helpers;
using OrderManagementSystem.Models;
using OrderManagementSystem.Repositories;

namespace OrderManagementSystem.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly string _jwtKey;

        public UserService(IUserRepository userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _jwtKey = config["Jwt:Key"]!;
        }

        public async Task<string> RegisterAsync(UserRegisterDto dto)
        {
            var exists = await _userRepository.GetByUsernameAsync(dto.Username);
            if (exists != null) throw new Exception("Username already exists");
            var user = new User
            {
                Username = dto.Username,
                PasswordHash = HashPassword(dto.Password),
                Role = dto.Role
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return JwtHelper.GenerateToken(user, _jwtKey);
        }

        public async Task<string> LoginAsync(UserLoginDto dto)
        {
            var user = await _userRepository.GetByUsernameAsync(dto.Username);
            if (user == null || user.PasswordHash != HashPassword(dto.Password))
                throw new Exception("Invalid credentials");

            return JwtHelper.GenerateToken(user, _jwtKey);
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
