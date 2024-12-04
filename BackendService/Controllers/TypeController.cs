using AutoMapper;
using BackendService.Models;
using BackendService.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace BackendService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeController : ControllerBase
    {
        private readonly MotoWebsiteContext _db;
        private readonly IMapper _mapper;

        public TypeController(MotoWebsiteContext context, IMapper mapper)
        {
            _db = context;
            _mapper = mapper;
        }


        [HttpGet("Types")]
        public async Task<IActionResult> GetMotoTypes()
        {
            var types = await _db.MotoTypes.ToListAsync();
            var mappedResult = _mapper.Map<List<TypeVM>>(types);
            return Ok(mappedResult);
        }

        [HttpGet("Types/{id}")]
        public async Task<IActionResult> GetMotoType(string id)
        {
            var type = await _db.MotoTypes.FindAsync(id);
            if (type == null)
            {
                return NotFound("Moto type not found.");
            }
            var mappedResult = _mapper.Map<TypeVM>(type);
            return Ok(mappedResult);
        }


        [HttpPost("Types")]
        public async Task<IActionResult> CreateMotoType([FromBody] TypeVM typeVM)
        {
            var typeID = _db.MotoTypes.FirstOrDefault(t => t.MaLoai.Equals(typeVM.MaLoai));
            var typeName = _db.MotoTypes.FirstOrDefault(t => t.TenLoai.Equals(typeVM.TenLoai));
            if (typeID == null && typeName == null)
            {
                var type = _mapper.Map<MotoType>(typeVM);
                _db.MotoTypes.Add(type); 
                await _db.SaveChangesAsync();
                var createdTypeVM = _mapper.Map<TypeVM>(type);
                return CreatedAtAction(nameof(GetMotoType), new { id = createdTypeVM.MaLoai }, createdTypeVM);
            }
            else return BadRequest("Mã, tên loại đã tồn tại");

           
        }

        // PUT: api/TypeAPI/{id}
        [HttpPut("Types/{id}")]
        public async Task<IActionResult> UpdateMotoType(string id, [FromBody] TypeVM typeVM)
        {
            if (id != typeVM.MaLoai)
            {
                return BadRequest("ID mismatch.");
            }
            var existingType = await _db.MotoTypes.FindAsync(id);
            if (existingType == null)
            {
                return NotFound("Type not found.");
            }
            var typeNameChecker = _db.MotoTypes.FirstOrDefault(b => b.TenLoai.Equals(typeVM.TenLoai));
            if (typeNameChecker != null && typeNameChecker.TenLoai != id)
            {
                return BadRequest("Tên Loại Đã Tồn Tại");
            }
            _mapper.Map(typeVM, existingType);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        // PATCH: api/TypeAPI/{id}
        [HttpPatch("Types/{id}")]
        public async Task<IActionResult> UpdatePartialMotoType(string id, [FromBody] JsonPatchDocument<TypeVM> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest("Invalid patch document.");
            }

            var existingType = await _db.MotoTypes.FindAsync(id);
            if (existingType == null)
            {
                return NotFound("Moto type not found.");
            }

            var typeToPatch = _mapper.Map<TypeVM>(existingType);
            patchDoc.ApplyTo(typeToPatch);

            if (!TryValidateModel(typeToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(typeToPatch, existingType);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/TypeAPI/{id}
        [HttpDelete("Types/{id}")]
        public async Task<IActionResult> DeleteMotoType(string id)
        {
            var type = await _db.MotoTypes.FindAsync(id);
            if (type == null)
            {
                return NotFound("Moto type not found.");
            }

            _db.MotoTypes.Remove(type);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
