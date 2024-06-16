using DataAccess.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Services.Text;

namespace FileSharingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TextController : ControllerBase
    {
        private readonly ITextService _textService;

        public TextController(ITextService textService)
        {
            _textService = textService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UploadText([FromBody] TextUploadDTO request)
        {
            var url = await _textService.UploadText(request.Content, request.AutoDelete, request.UserId);
            return Ok(new { Url = url });
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
