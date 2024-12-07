using AutoMapper;
using BackendService.Models;
using BackendService.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorController : ControllerBase
    {
        private readonly MotoWebsiteContext _db;
        private readonly ILogger<ColorController> _logger;
        private readonly IMapper _mapper;

        public ColorController(MotoWebsiteContext context, ILogger<ColorController> logger, IMapper mapper)
        {
            _db = context;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("Colors/{id}")]
        public async Task<IActionResult> GetColor(string id)
        {
            var versionColor = await _db.VersionColors
                .Where(c => c.MaVersionColor == id)   
                .Include(i => i.VersionImages)
                .FirstOrDefaultAsync();
            var mappedResult = _mapper.Map<VersionColorVM>(versionColor);
            return Ok(mappedResult);
        }

        [HttpPost("Colors")]
        public async Task<IActionResult> CreateColor([FromBody] VersionColorVM colorVM)
        {
            var versionIDCheck = _db.VersionColors.FirstOrDefault(c=>c.MaVersionColor==colorVM.MaVersionColor);
            if (versionIDCheck != null) return BadRequest("Mã color bị trùng");
            else
            {
                var newColor = _mapper.Map<VersionColor>(colorVM);
                _db.VersionColors.Add(newColor);
                await _db.SaveChangesAsync();
                var createdColor = _mapper.Map<VersionColorVM>(newColor);
                return CreatedAtAction(nameof(GetColor), new { id = newColor.MaVersionColor }, createdColor);
            }
        }

        [HttpPut("Colors/{id}")]
        public async Task<IActionResult> UpdateColor(string id, [FromBody] VersionColorVM colorVM)
        {
            if (id != colorVM.MaVersionColor)
            {
                return BadRequest("ID mismatch.");
            }

            var existingVersion = await _db.VersionColors
                .FirstOrDefaultAsync(v => v.MaVersion == id);

            if (existingVersion == null)
            {
                return NotFound("Version not found.");
            }

            _mapper.Map(colorVM, existingVersion);

            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("Colors/{id}")]
        public async Task<IActionResult> DeleteColor(string id)
        {
            var color =await _db.VersionColors.FirstOrDefaultAsync(c=>c.MaVersionColor == id);

            if (color == null)
            {
                return NotFound("Color not found.");
            }

            // Xóa các đối tượng con trước khi xóa đối tượng cha
            else
            {
                var imageToRemove = _db.VersionImages.Where(i => i.MaVersionColor == id).ToList();
                _db.VersionImages.RemoveRange(imageToRemove);
                _db.VersionColors.Remove(color);
                await _db.SaveChangesAsync();
            }
            return NoContent();
        }
    }


}
