using BackendService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using BackendService.ViewModels;
using Newtonsoft.Json;
using Microsoft.AspNetCore.JsonPatch;
using X.PagedList;

namespace BackendService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MotoController : ControllerBase
    {
        private readonly MotoWebsiteContext _db;
        private readonly ILogger<MotoController> _logger;
        private readonly IMapper _mapper;
        public MotoController(MotoWebsiteContext context, ILogger<MotoController> logger, IMapper mapper)
        {
            _db = context;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("Motos")]
        public async Task<IActionResult> GetMotos()
        {
            var motos = await _db.MotoBikes
                .Include(m => m.MotoVersions)
                .ThenInclude(v => v.VersionColors)
                .ThenInclude(vc => vc.VersionImages)
                .Include(m => m.MaLibraryNavigation)
                .ThenInclude(l => l.LibraryImages)
                .ToListAsync();

            var mappedResult = _mapper.Map<List<MotoVM>>(motos);
            return Ok(mappedResult);
        }


        [HttpGet("Motos/{id}")]
        public async Task<IActionResult> GetMoto(string id)
        {
            var moto = await _db.MotoBikes.Where(m=>m.MaXe == id)
                .Include(m => m.MotoVersions)
                .ThenInclude(v => v.VersionColors)        
                .ThenInclude(vc => vc.VersionImages)
                .Include(l=>l.MaLibraryNavigation)
                .ThenInclude(li=>li.LibraryImages)
                .Include(h=>h.MaHangSanXuatNavigation)
                .Include(t=>t.MaLoaiNavigation)
                .FirstOrDefaultAsync();

            if (moto == null)
            {
                return NotFound("Moto not found.");
            }
            var mappedResult = _mapper.Map<MotoVM>(moto);
            return Ok(mappedResult);
        }

        [HttpPut("Motos/{id}")]
        public async Task<IActionResult> UpdateMoto(string id, [FromBody] MotoVM motoVM)
        {
            if (id != motoVM.MaXe)
            {
                return BadRequest("ID mismatch.");
            }

            var existingMoto = await _db.MotoBikes.Include(m => m.MotoVersions)
                    .ThenInclude(v => v.VersionColors)
                    .ThenInclude(vc => vc.VersionImages)
                    .Include(m => m.MaLibraryNavigation)
                    .ThenInclude(l => l.LibraryImages)
                    .FirstOrDefaultAsync(m => m.MaXe == id);

            if (existingMoto == null)
            {
                return NotFound("Moto not found.");
            }

            _mapper.Map(motoVM, existingMoto);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MotoExists(motoVM.MaXe))
                {
                    return NotFound("Moto not found.");
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        private bool MotoExists(string id)
        {
            return _db.MotoBikes.Any(e => e.MaXe == id);
        }

        [HttpDelete("Motos/{id}")]
        public async Task<IActionResult> DeleteMoto(string id)
        {
            var moto = await _db.MotoBikes
                .Include(m => m.MotoVersions)
                .ThenInclude(v => v.VersionColors)
                .ThenInclude(vc => vc.VersionImages)
                .Include(m => m.MaLibraryNavigation)
                .ThenInclude(l => l.LibraryImages)
                .FirstOrDefaultAsync(m => m.MaXe == id);

            if (moto == null)
            {
                return NotFound("Moto not found.");
            }

            _db.MotoBikes.Remove(moto);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("Motos")]
        public async Task<IActionResult> CreateMoto([FromBody] MotoVM motoVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newMoto = _mapper.Map<MotoBike>(motoVM);

            _db.MotoBikes.Add(newMoto);
            await _db.SaveChangesAsync();

            var createdMoto = _mapper.Map<MotoVM>(newMoto);

            return CreatedAtAction(nameof(GetMoto), new { id = newMoto.MaXe }, createdMoto);
        }
    }
}
