

namespace BKZalo.Api.Authentication
{
    public interface IJwtAuthenticationManager
    {
        string Authenticate(string phoneNumber, string password);
    }
}
