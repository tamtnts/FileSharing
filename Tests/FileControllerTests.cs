using Moq;
using Microsoft.AspNetCore.Mvc;
using Services.Services.File;
using FileSharingAPI.Controllers;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Tests
{
    public class FileControllerTests
    {
        private readonly Mock<IFileService> _mockFileService;
        private readonly FileController _controller;

        public FileControllerTests()
        {
            _mockFileService = new Mock<IFileService>();
            _controller = new FileController(_mockFileService.Object);
        }

        [Fact]
        public async Task UploadFile_ValidFile_ReturnsOkResult()
        {
            var fileMock = new Mock<IFormFile>();
            var fileName = "testfile.txt";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write("test content");
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);
            var autoDelete = false;
            var userId = 1;

            var claims = new List<Claim>
        {
            new Claim("UserId", userId.ToString())
        };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var principal = new ClaimsPrincipal(identity);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = principal }
            };

            _mockFileService.Setup(service => service.UploadFile(It.IsAny<IFormFile>(), userId, autoDelete)).ReturnsAsync("http://example.com/testfile.txt");

            var result = await _controller.UploadFile(fileMock.Object, autoDelete);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("http://example.com/testfile.txt", ((dynamic)okResult.Value).Url);
        }

        [Fact]
        public async Task UploadFile_InvalidFile_ReturnsBadRequest()
        {
            IFormFile file = null;
            var autoDelete = false;

            var result = await _controller.UploadFile(file, autoDelete);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("File is empty or not provided", badRequestResult.Value);
        }

        [Fact]
        public async Task UploadFile_InvalidUserId_ReturnsUnauthorized()
        {
            // Mock IFormFile
            var fileMock = new Mock<IFormFile>();
            var fileName = "testfile.txt";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write("test content");
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);

            // Mock HttpContext
            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
        new Claim("UserId", "123")  // Replace "123" with the desired user ID for testing
            }, "mock"));

            // Assign mocked HttpContext to the controller
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var autoDelete = false;

            // Call the controller action
            var result = await _controller.UploadFile(fileMock.Object, autoDelete);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Invalid user ID.", unauthorizedResult.Value as string);
        }


        [Fact]
        public async Task GetFile_ValidId_ReturnsFile()
        {
            var fileId = Guid.NewGuid();
            var fileName = "testfile.txt";
            var file = new DataAccess.Models.File { FileName = fileName };
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write("test content");
            writer.Flush();
            ms.Position = 0;

            _mockFileService.Setup(service => service.GetFileById(fileId)).ReturnsAsync(file);
            _mockFileService.Setup(service => service.DownloadFile(fileName)).ReturnsAsync(ms);

            var result = await _controller.GetFile(fileId);

            var fileStreamResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal("application/octet-stream", fileStreamResult.ContentType);
            Assert.Equal(fileName, fileStreamResult.FileDownloadName);
        }

        [Fact]
        public async Task GetFile_InvalidId_ReturnsNotFound()
        {
            var fileId = Guid.NewGuid();

            _mockFileService.Setup(service => service.GetFileById(fileId)).ThrowsAsync(new FileNotFoundException());

            var result = await _controller.GetFile(fileId);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
