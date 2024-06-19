using DataAccess.DTO;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Services.Text;
using Services.Utils;
using System.Security.Claims;

namespace FileSharingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TextController : ControllerBase
    {
        private readonly ITextService _textService;
        private DecodeToken _decodeToken;

        public TextController(ITextService textService, DecodeToken decodeToken)
        {
            _textService = textService;
            _decodeToken = decodeToken;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UploadText([FromBody] TextUploadDTO request)
        {
            if (User.Identity is ClaimsIdentity identity)
            {
                var userIdClaim = identity.FindFirst("UserId");
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    var url = await _textService.UploadText(request.Content, request.AutoDelete, userId);
                    return Ok(new { Url = url });
                }
                else
                {
                    return Unauthorized("Invalid user ID claim.");
                }
            }
            return Unauthorized();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetText(Guid id)
        {
            try
            {
                var text = await _textService.GetTextById(id);
                return Ok(text.TextContent);
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
