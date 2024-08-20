namespace Web.Api.Users;

public sealed class LoginUser(IUserRepository userRepository, IPasswordHasher passwordHasher)
{
    public record Request(string Email, string Password);

    public async Task<User> Handle(Request request)
    {
        User? user = await userRepository.GetByEmail(request.Email);

        if (user is null)
        {
            throw new Exception("The user was not found");
        }

        bool verified = passwordHasher.Verify(request.Password, user.PasswordHash);

        if (!verified)
        {
            throw new Exception("The password is incorrect");
        }

        return user;
    }
}
