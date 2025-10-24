namespace Recruitment.Application.DTOs;

public class RegisterDto
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
}

public class LoginDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class RefreshTokenDto
{
    public string Token { get; set; }
}