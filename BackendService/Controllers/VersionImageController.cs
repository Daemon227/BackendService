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
    public class VersionImageController : ControllerBase
    {
        private readonly MotoWebsiteContext _db;
        private readonly ILogger<VersionImageController> _logger;
        private readonly IMapper _mapper;

        public VersionImageController(MotoWebsiteContext context, ILogger<VersionImageController> logger, IMapper mapper)
        {
            _db = context;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost("VersionImages")]
        public async Task<IActionResult> CreateImage(VersionImageVM versionImage)
        {
            var mappedResult = _mapper.Map<VersionImage>(versionImage);
            _db.VersionImages.Add(mappedResult);
            _db.SaveChanges();
            return Ok(mappedResult);
        }
    }
}
