using AutoMapper;
using BackendService.Models;
using BackendService.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace BackendService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        private readonly MotoWebsiteContext _db;
        private readonly ILogger<MotoController> _logger;
        private readonly IMapper _mapper;
        public VersionController(MotoWebsiteContext context, ILogger<MotoController> logger, IMapper mapper)
        {
            _db = context;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("{motoID}/Versions")]
        public async Task<IActionResult> GetVersions(string motoID)
        {
            var versions = await _db.MotoVersions
                .Where(v=>v.MaXe== motoID)
                .Include(v => v.VersionColors)
                .ThenInclude(vc => vc.VersionImages)
                .ToListAsync();   
            var mappedResult = _mapper.Map<List<VersionVM>>(versions);
            return Ok(mappedResult);
        }

        [HttpGet("Versions/{id}")]
        public async Task<IActionResult> GetVersion(string id)
        {
            var version = await _db.MotoVersions
                .Include(v => v.VersionColors)
                .ThenInclude(vc => vc.VersionImages)
                .FirstOrDefaultAsync(v => v.MaVersion == id);

            if (version != null)
            {
                var result = _mapper.Map<VersionVM>(version);
                return Ok(result);
            }
            else
            {
                return NotFound("Khong tim thay");
            }
        }

        [HttpPost("Versions")]
        public async Task<IActionResult> CreateVersion([FromBody] VersionVM versionVM)
        {
            var versionIDCheck = _db.MotoVersions.FirstOrDefault(v => v.MaVersion == versionVM.MaVersion);
            if (versionIDCheck != null) return BadRequest("Mã version bị trùng");
            else
            {
                var newVersion = _mapper.Map<MotoVersion>(versionVM);
                _db.MotoVersions.Add(newVersion);
                await _db.SaveChangesAsync();
                var createdVersion = _mapper.Map<VersionVM>(newVersion);
                return CreatedAtAction(nameof(GetVersion), new { id = newVersion.MaXe}, createdVersion);
            }
           
        }

        [HttpPut("Versions/{id}")]
        public async Task<IActionResult> UpdateVersion(string id, [FromBody] VersionVM versionVM)
        {
            if (id != versionVM.MaVersion)
            {
                return BadRequest("ID mismatch.");
            }

            var existingVersion = await _db.MotoVersions
                .FirstOrDefaultAsync(v => v.MaVersion == id);

            if (existingVersion == null)
            {
                return NotFound("Version not found.");
            }

            _mapper.Map(versionVM, existingVersion);

            await _db.SaveChangesAsync();
            return NoContent();
        }

        

        [HttpDelete("Versions/{id}")]
        public async Task<IActionResult> DeleteVersion(string id)
        {
            var version = await _db.MotoVersions
                .Include(v => v.VersionColors)
                .ThenInclude(vc => vc.VersionImages)
                .FirstOrDefaultAsync(v => v.MaVersion == id);

            if (version == null)
            {
                return NotFound("Version not found.");
            }

			// Xóa các đối tượng con trước khi xóa đối tượng cha
			_db.VersionImages.RemoveRange(version.VersionColors.SelectMany(vc => vc.VersionImages));
			_db.VersionColors.RemoveRange(version.VersionColors);
            _db.MotoVersions.Remove(version);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
