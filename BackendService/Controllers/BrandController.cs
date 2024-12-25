using AutoMapper;
using BackendService.Models;
using BackendService.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly MotoWebsiteContext _db;
        private readonly IMapper _mapper;
        public BrandController(MotoWebsiteContext context, IMapper mapper)
        {
            _db = context;
            _mapper = mapper;
        }

        [HttpGet("Brands")]
        public async Task<IActionResult> GetBrands()
        {
            var brands = await _db.Brands.ToListAsync();
            var mappedResult = _mapper.Map<List<BrandVM>>(brands);
            return Ok(mappedResult);
        }

        [HttpGet("Brands/{id}")]
        public async Task<IActionResult> GetBrand(string id)
        {
            var brand = await _db.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound("Brand not found.");
            }
            var mappedResult = _mapper.Map<BrandVM>(brand);
            return Ok(mappedResult);
        }

        [HttpPost("Brands")]
        public async Task<IActionResult> CreateBrand([FromBody] BrandVM brandVM)
        {
            var brandID = _db.Brands.FirstOrDefault(b => b.MaHangSanXuat.Equals(brandVM.MaHangSanXuat));
            var brandName = _db.Brands.FirstOrDefault(b => b.TenHangSanXuat.Equals(brandVM.TenHangSanXuat));
            if (brandID == null && brandName ==null)
            {
                var brand = _mapper.Map<Brand>(brandVM);
                _db.Brands.Add(brand); 
                await _db.SaveChangesAsync();
                var createdBrand = _mapper.Map<BrandVM>(brand);
                return CreatedAtAction(nameof(GetBrand), new { id = createdBrand.MaHangSanXuat }, createdBrand);
            }
            else return BadRequest("Mã, tên hãng Không được trùng");           
        }

        [HttpPut("Brands/{id}")]
        public async Task<IActionResult> UpdateBrand(string id, [FromBody] BrandVM brandVM)
        {
            if (id != brandVM.MaHangSanXuat)
            {
                return BadRequest("ID mismatch.");
            }
            var existingBrand = await _db.Brands.FindAsync(id);
            if (existingBrand == null)
            {
                return NotFound("Brand not found.");
            }
            var brandName = _db.Brands.FirstOrDefault(b => b.TenHangSanXuat.Equals(brandVM.TenHangSanXuat));
            if (brandName != null && brandName.MaHangSanXuat != id)
            {
                return BadRequest("Tên Hãng Bị Trùng");
            }
            _mapper.Map(brandVM, existingBrand);
            await _db.SaveChangesAsync(); 
            return NoContent();
        }

        [HttpPatch("Brands/{id}")]
        public async Task<IActionResult> UpdatePartialBrand(string id, [FromBody] JsonPatchDocument<BrandVM> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest("Invalid patch document.");
            }
            var existingBrand = await _db.Brands.FindAsync(id);
            if (existingBrand == null)
            {
                return NotFound("Brand not found.");
            }
            var brandToPatch = _mapper.Map<BrandVM>(existingBrand);
            patchDoc.ApplyTo(brandToPatch);
            if (!TryValidateModel(brandToPatch))
            {
                return ValidationProblem(ModelState);
            }
            _mapper.Map(brandToPatch, existingBrand);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("Brands/{id}")]
        public async Task<IActionResult> DeleteBrand(string id)
        {
            var brand = await _db.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound("Brand not found.");
            }
            _db.Brands.Remove(brand);
            await _db.SaveChangesAsync();
            return NoContent();
        }


    }
}
