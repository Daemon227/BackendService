using AutoMapper;
using BackendService.Models;
using BackendService.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MotoWebsiteContext _db;
        private readonly IMapper _mapper;
        public UserController(MotoWebsiteContext context, IMapper mapper)
        {
            _db = context;
            _mapper = mapper;
        }

        [HttpGet("Users/{Username}")]
        public async Task<IActionResult> GetUser(string Username)
        {
            var user = _db.Users.FirstOrDefault(u => u.Username == Username);
            if (user == null)
            {
                return NotFound("Không tìm thấy tài khoản");
            }
            var mappedResult = _mapper.Map<UserVM>(user);
            return Ok(mappedResult);
        }

        [HttpPost("Users")]
        public async Task<IActionResult> CreateBrand([FromBody] UserVM userVM)
        {
            var user = _db.Users.FirstOrDefault(u => u.Username == userVM.Username);
            if (user == null)
            {
                var newUser = _mapper.Map<User>(userVM);
                _db.Users.Add(newUser);
                await _db.SaveChangesAsync();
                var createdUser = _mapper.Map<UserVM>(user);
                return Ok();
            }
            else return BadRequest("Tên tài khoản đã tồn tại");
        }


    }
}
