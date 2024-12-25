using AutoMapper;
using BackendService.Models;
using BackendService.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackendService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryController : ControllerBase
    {
        private readonly MotoWebsiteContext _db;
        private readonly ILogger<LibraryController> _logger;
        private readonly IMapper _mapper;

        public LibraryController(MotoWebsiteContext context, ILogger<LibraryController> logger, IMapper mapper)
        {
            _db = context;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("Libraries/{id}")]
        public async Task<IActionResult> GetLibrary(string id)
        {
            var library = await _db.MotoLibraries
				.Where(l=>l.MaLibrary ==id)      
				.FirstOrDefaultAsync();
            if (library == null) {
                return BadRequest("Không tìm thấy library");
            }
            else
            {
				var libraryImage = await _db.LibraryImages.Where(i => i.MaLibrary == library.MaLibrary).ToListAsync();
				library.LibraryImages = libraryImage;
                var mappedResult = _mapper.Map<LibraryVM>(library);
                return Ok(mappedResult);
            }
        }

		[HttpPost("Libraries")]
		public async Task<IActionResult> CreateLibrary([FromBody] LibraryVM libraryVM)
		{
			var library = _db.MotoLibraries.FirstOrDefault(l=>l.MaLibrary == libraryVM.MaLibrary);
			if (library == null)
			{
				var l = _mapper.Map<MotoLibrary>(libraryVM);
				_db.MotoLibraries.Add(l);      

				await _db.SaveChangesAsync();
				var createLibrary = _mapper.Map<LibraryVM>(l);
				return CreatedAtAction(nameof(GetLibrary), new { id = createLibrary.MaLibrary }, createLibrary);
			}
			else return BadRequest("Mã, thư viện Không được trùng");
		}

		[HttpDelete("Libraries/{id}")]
		public async Task<IActionResult> DeleteLibrary(string id)
		{
			var library = await _db.MotoLibraries.FindAsync(id);
			if (library == null)
			{
				return NotFound("Library not found.");
			}
			var imgages = await _db.LibraryImages.Where(i => i.MaLibrary == library.MaLibrary).ToListAsync();
			_db.LibraryImages.RemoveRange(imgages);
			_db.MotoLibraries.Remove(library);
			await _db.SaveChangesAsync();
			return NoContent();
		}

        [HttpDelete("ResetLibraries/{id}")]
        public async Task<IActionResult> DeleteImagesLibrary(string id)
        {
            var library = await _db.MotoLibraries.FindAsync(id);
            if (library == null)
            {
                return NotFound("Library not found.");
            }
            var imgages = await _db.LibraryImages.Where(i => i.MaLibrary == library.MaLibrary).ToListAsync();
            _db.LibraryImages.RemoveRange(imgages);   
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("Libraries/{id}")]
        public async Task<IActionResult> UpdateLibrary(string id, [FromBody] LibraryVM libraryVM)
        {
            var library = await _db.MotoLibraries.FindAsync(id);
            if (library == null)
            {
                return NotFound("Library not found.");
            }
            if (libraryVM.LibraryImageVM != null)
            {
                foreach (var img in libraryVM.LibraryImageVM)
                {
                   /* var libraryImage = new LibraryImage
                    {
                        MaLibrary = img.MaLibrary,
                        ImageUrl = img.ImageUrl,
                    };
                    _db.LibraryImages.Add(libraryImage);*/
                   var newImg = _mapper.Map<LibraryImage>(img);
                    library.LibraryImages.Add(newImg);
                }
            }
            else return NotFound("libraryVM is null");
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
