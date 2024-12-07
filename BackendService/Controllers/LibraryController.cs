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
            var library = await _db.MotoLibraries.Where(l=>l.MaLibrary ==id)
                .Include(i=>i.LibraryImages).FirstOrDefaultAsync();
            if (library == null) {
                return BadRequest("Không tìm thấy library");
            }
            else
            {
                var mappedResult = _mapper.Map<LibraryVM>(library);
                return Ok(mappedResult);
            }
        }
    }
}
