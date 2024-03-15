namespace retonet.Models
{
    // Asegúrate de que las propiedades no sean nulas.
public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
}