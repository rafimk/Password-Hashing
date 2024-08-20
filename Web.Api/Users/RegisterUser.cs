namespace Web.Api.Users;

public sealed class RegisterUser(IUserRepository userRepository, IPasswordHasher passwordHasher)
{
    public sealed record Request(string Email, string FirstName, string LastName, string Password);

    public async Task<User> Handle(Request request)
    {
        // Race conditions?
        if (await userRepository.Exists(request.Email))
        {
            throw new Exception("The email is already in use");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PasswordHash = passwordHasher.Hash(request.Password)
        };

        await userRepository.Insert(user);

        // Email verification?
        // Access token?

        return user;
    }
}
