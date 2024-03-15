namespace retonet.Services
{
    using retonet.Models;
    public interface IUserService
    {
        Task<User?> GetUserByEmailAsync(string email);
        bool VerifyPassword(string providedPassword, string storedHash, string salt);
        string GenerateJwtToken(User user);
    }
}