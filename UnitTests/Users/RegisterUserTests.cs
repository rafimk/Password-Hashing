using FluentAssertions;
using NSubstitute;
using Web.Api.Users;

namespace UnitTests.Users;

public class RegisterUserTests
{
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IPasswordHasher _passwordHasher = Substitute.For<IPasswordHasher>();
    private readonly RegisterUser _handler;

    public RegisterUserTests()
    {
        _handler = new RegisterUser(_userRepository, _passwordHasher);
    }

    [Fact]
    public async Task Handle_WithUniqueEmail_CreatesAndInsertsUser()
    {
        // Arrange
        var request = new RegisterUser.Request("test@example.com", "John", "Doe", "password123");
        _userRepository.Exists(request.Email).Returns(false);
        _passwordHasher.Hash(request.Password).Returns("hashed_password");

        // Act
        User user = await _handler.Handle(request);

        // Assert
        user.Should().NotBeNull();
        user.Email.Should().Be(request.Email);
        user.FirstName.Should().Be(request.FirstName);
        user.LastName.Should().Be(request.LastName);
        user.PasswordHash.Should().Be("hashed_password");

        await _userRepository.Received(1).Insert(user);
    }

    [Fact]
    public async Task Handle_WithDuplicateEmail_ThrowsException()
    {
        // Arrange
        var request = new RegisterUser.Request("duplicate@example.com", "Jane", "Smith", "securepass");
        _userRepository.Exists(request.Email).Returns(true);

        // Act
        Func<Task> act = async () => await _handler.Handle(request);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("The email is already in use");
        await _userRepository.DidNotReceive().Insert(Arg.Any<User>());
    }
}
