using Microsoft.EntityFrameworkCore;
using retonet.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net; // Asegúrate de tener BCrypt.Net como dependencia para usar BCrypt

namespace retonet.Services
{
    public class UserService : IUserService
    {
        private readonly RetonetDbContext _context; // Usa el nombre real de tu clase DbContext

        public UserService(RetonetDbContext context)
        {
            _context = context;
        }

        public bool VerifyPassword(string providedPassword, string storedHash, string salt)
    {
    // Asegúrate de que providedPassword, storedHash y salt no sean nulos antes de usarlos
            if (providedPassword == null || storedHash == null || salt == null)
            throw new ArgumentNullException(providedPassword == null ? nameof(providedPassword) : storedHash == null ? nameof(storedHash) : nameof(salt));

        return BCrypt.Net.BCrypt.Verify(providedPassword + salt, storedHash);
    }
        public string GenerateJwtToken(User user)
        {
            // Generación y devolución de un token JWT para el usuario
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("VjckP5TM8v"); // Reemplaza con tu clave secreta real
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Email ?? "emailDesconocido"),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

            public async Task<User?> GetUserByEmailAsync(string email)
        {
            if (email == null) throw new ArgumentNullException(nameof(email));
            return _context.Users != null ? await _context.Users.FirstOrDefaultAsync(u => u.Email == email) : null;
        }


    }
}