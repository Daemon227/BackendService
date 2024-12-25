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

            var moto = await _db.MotoBikes.FindAsync(id);
            
            if (moto == null)
            {
                return NotFound("Moto not found.");
            }

            _mapper.Map(motoVM, moto);
            await _db.SaveChangesAsync();   
            return NoContent();
        }

        private bool MotoExists(string id)
        {
            return _db.MotoBikes.Any(e => e.MaXe == id);
        }

        [HttpDelete("Motos/{id}")]
        public async Task<IActionResult> DeleteMoto(string id)
        {
            var moto = await _db.MotoBikes.FirstOrDefaultAsync(m => m.MaXe == id);

            if (moto == null)
            {
                return NotFound("Moto not found.");
            }
            //xoa version: de sau
            var version = await _db.MotoVersions.Where(v=>v.MaXe == moto.MaXe).ToListAsync();
            if (version.Any()) 
            { 
                foreach (var v in version)
                {
                    var versionColor = await _db.VersionColors.Where(c=>c.MaVersion == v.MaVersion).ToListAsync();
                    if (versionColor.Any())
                    {
                        foreach (var c in versionColor)
                        {
                            var images = await _db.VersionImages.Where(i => i.MaVersionColor == c.MaVersionColor).ToListAsync();
                            if (images != null && images.Count > 0)
                            {
                                _db.VersionImages.RemoveRange(images);
                            }
                        }
                        _db.VersionColors.RemoveRange(versionColor);
                    }
                }            
                _db.MotoVersions.RemoveRange(version);
            }

            //xoa library
            var library = await _db.MotoLibraries.FirstOrDefaultAsync(m => m.MaLibrary == moto.MaLibrary);
            if (library != null) 
            {
				var images = await _db.LibraryImages.Where(i => i.MaLibrary == library.MaLibrary).ToListAsync();
                if (images != null && images.Count > 0) 
                {
					_db.LibraryImages.RemoveRange(images);
				}	
				_db.MotoLibraries.Remove(library);
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
            var moto =_db.MotoBikes.FirstOrDefault(m=>m.TenXe == motoVM.TenXe);
            if (moto == null)
            {
                var newMoto = _mapper.Map<MotoBike>(motoVM);

                _db.MotoBikes.Add(newMoto);
                await _db.SaveChangesAsync();

                var createdMoto = _mapper.Map<MotoVM>(newMoto);

                return CreatedAtAction(nameof(GetMoto), new { id = newMoto.MaXe }, createdMoto);
            }
            else return BadRequest("Tên xe đã tồn tại");
        }


    }
}
