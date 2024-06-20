using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Services.File;
using System.Security.Claims;

namespace FileSharingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UploadFile(IFormFile file, [FromForm] bool autoDelete)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is empty or not provided");

            if (User.Identity is ClaimsIdentity identity)
            {
                var userIdClaim = identity.FindFirst("UserId");
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    var url = await _fileService.UploadFile(file, userId, autoDelete);
                    return Ok(new { Url = url });
                }
                else
                {
                    return Unauthorized("Invalid user ID.");
                }
            }
            return Unauthorized();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFile(Guid id)
        {
            try
            {
                var file = await _fileService.GetFileById(id);
                var stream = await _fileService.DownloadFile(file.FileName);
                return new FileStreamResult(stream, "application/octet-stream")
                {
                    FileDownloadName = file.FileName
                };
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
