using DataAccess.DTO;
using FileSharingAPI.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json.Linq;
using Services.Services.Text;
using Services.Utils;
using System.Security.Claims;

namespace Tests
{
    public class TextControllerTests
    {
        private readonly TextController _controller;
        private readonly Mock<ITextService> _textServiceMock;
        private readonly Mock<DecodeToken> _decodeTokenMock;

        public TextControllerTests()
        {
            _textServiceMock = new Mock<ITextService>();
            _decodeTokenMock = new Mock<DecodeToken>();
            _controller = new TextController(_textServiceMock.Object, _decodeTokenMock.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("UserId", "1")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Fact]
        public async Task UploadText_ValidRequest_ReturnsOk()
        {
            var request = new TextUploadDTO { Content = "Sample Text", AutoDelete = false };
            _textServiceMock.Setup(service => service.UploadText(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>()))
                .ReturnsAsync("https://mockurl.com/text");

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, "testuser"),
        new Claim("UserId", "1")
    };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            var result = await _controller.UploadText(request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var json = JObject.FromObject(okResult.Value);
            Assert.NotNull(json); // Ensure the response is not null
            Assert.Equal("https://mockurl.com/text", json["Url"].ToString());
        }

        [Fact]
        public async Task UploadText_InvalidUserIdClaim_ReturnsUnauthorized()
        {
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            var result = await _controller.UploadText(new TextUploadDTO { Content = "Sample Text", AutoDelete = false });

            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Invalid user ID claim.", unauthorizedResult.Value);
        }

        [Fact]
        public async Task UploadText_NoUserClaims_ReturnsUnauthorized()
        {
            // Arrange
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            // Act
            var result = await _controller.UploadText(new TextUploadDTO { Content = "Sample Text", AutoDelete = false });

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Invalid user ID claim.", unauthorizedResult.Value);
        }


        [Fact]
        public async Task GetText_ValidId_ReturnsOk()
        {
            var textId = Guid.NewGuid();
            _textServiceMock.Setup(service => service.GetTextById(textId))
                .ReturnsAsync(new DataAccess.Models.Text { TextContent = "Sample Text" });

            var result = await _controller.GetText(textId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Sample Text", okResult.Value);
        }

        [Fact]
        public async Task GetText_InvalidId_ReturnsNotFound()
        {
            var textId = Guid.NewGuid();
            _textServiceMock.Setup(service => service.GetTextById(textId))
                .Throws<FileNotFoundException>();

            var result = await _controller.GetText(textId);

            var notFoundResult = Assert.IsType<NotFoundResult>(result);
        }
    }
}
