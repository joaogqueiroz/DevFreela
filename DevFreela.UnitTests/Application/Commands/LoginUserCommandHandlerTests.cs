using Moq;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using DevFreela.Core.Services;
using DevFreela.Application.Commands.LoginUser;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
namespace DevFreela.UnitTests.Application.Commands
{
  public class LoginUserCommandHandlerTests
  {
    private const string Email = "client@test.com";
    private const string HashedPassword = "hashed-password";

    private static (Mock<IUserRepository>, Mock<IAuthService>) CreateMocks(bool passwordMatches)
    {
      var userRepositoryMock = new Mock<IUserRepository>();
      userRepositoryMock
        .Setup(ur => ur.GetByEmailAsync(Email))
        .ReturnsAsync(new User("Client", Email, new DateTime(1990, 1, 1), HashedPassword, "client"));

      var authServiceMock = new Mock<IAuthService>();
      authServiceMock
        .Setup(a => a.VerifyPassword(HashedPassword, It.IsAny<string>()))
        .Returns(passwordMatches);
      authServiceMock
        .Setup(a => a.GenerateJwtToken(Email, "client"))
        .Returns("token");

      return (userRepositoryMock, authServiceMock);
    }

    [Fact]
    public async Task RightPassword_Return_Token()
    {
      //Arrange
      var (userRepositoryMock, authServiceMock) = CreateMocks(passwordMatches: true);
      var handler = new LoginUserCommandHandler(authServiceMock.Object, userRepositoryMock.Object);

      //Act
      var result = await handler.Handle(new LoginUserCommand { Email = Email, Password = "Senha@123" }, new CancellationToken());

      //Assert
      Assert.NotNull(result);
      Assert.Equal("token", result.Token);
      authServiceMock.Verify(a => a.VerifyPassword(HashedPassword, "Senha@123"), Times.Once);
    }

    [Fact]
    public async Task WrongPassword_Return_Null()
    {
      //Arrange
      var (userRepositoryMock, authServiceMock) = CreateMocks(passwordMatches: false);
      var handler = new LoginUserCommandHandler(authServiceMock.Object, userRepositoryMock.Object);

      //Act
      var result = await handler.Handle(new LoginUserCommand { Email = Email, Password = "wrong" }, new CancellationToken());

      //Assert
      Assert.Null(result);
      authServiceMock.Verify(a => a.GenerateJwtToken(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task UnknownEmail_Return_Null()
    {
      //Arrange
      var (userRepositoryMock, authServiceMock) = CreateMocks(passwordMatches: true);
      var handler = new LoginUserCommandHandler(authServiceMock.Object, userRepositoryMock.Object);

      //Act
      var result = await handler.Handle(new LoginUserCommand { Email = "nobody@test.com", Password = "Senha@123" }, new CancellationToken());

      //Assert
      Assert.Null(result);
      authServiceMock.Verify(a => a.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }
  }
}
