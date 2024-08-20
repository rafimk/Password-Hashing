namespace Web.Api.Users;

public interface IPasswordHasher
{
    string Hash(string password);
}
