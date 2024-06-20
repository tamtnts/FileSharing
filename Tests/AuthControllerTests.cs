using Moq;
using Microsoft.AspNetCore.Mvc;
using Services.Services.User;
using DataAccess.DTO;
using FileSharingAPI.Controllers;

namespace Tests
{
    public class AuthControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _controller = new AuthController(_mockUserService.Object);
        }

        [Fact]
        public async Task Register_ValidUser_ReturnsOkResult()
        {
            // Arrange
            var userDto = new UserDTO { Email = "testuser", Password = "Password123" };
            _mockUserService.Setup(service => service.Register(It.IsAny<UserDTO>())).ReturnsAsync("token");

            // Act
            var result = await _controller.Register(userDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("token", okResult.Value);
        }

        [Fact]
        public async Task Register_InvalidUser_ReturnsBadRequest()
        {
            // Arrange
            var userDto = new UserDTO { Email = "", Password = "Password123" };
            _mockUserService.Setup(service => service.Register(It.IsAny<UserDTO>())).ThrowsAsync(new ArgumentException("Invalid user"));

            // Act
            var result = await _controller.Register(userDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid user", badRequestResult.Value);
        }

        [Fact]
        public async Task Login_ValidUser_ReturnsOkResult()
        {
            // Arrange
            var userDto = new UserDTO { Email = "testuser", Password = "Password123" };
            _mockUserService.Setup(service => service.Login(It.IsAny<UserDTO>())).ReturnsAsync("token");

            // Act
            var result = await _controller.Login(userDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("token", okResult.Value);
        }

        [Fact]
        public async Task Login_InvalidUser_ReturnsBadRequest()
        {
            // Arrange
            var userDto = new UserDTO { Email = "testuser", Password = "wrongpassword" };
            _mockUserService.Setup(service => service.Login(It.IsAny<UserDTO>())).ThrowsAsync(new ArgumentException("Invalid credentials"));

            // Act
            var result = await _controller.Login(userDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid credentials", badRequestResult.Value);
        }
    }
}