namespace DevFreela.Core.Services
{
  public interface IAuthService
  {
    string GenerateJwtToken(string email, string role);
    string HashPassword(string password);
    bool VerifyPassword(string hashedPassword, string password);
  }
}