using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BackendService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
		[HttpPost]
		[Route("MotoImages")]
		public async Task<IActionResult> UploadMotoFiles([FromForm] List<IFormFile> files)
		{
			var savedFileNames = new List<string>();

			if (files == null || files.Count == 0)
			{
				return BadRequest(new { success = false, message = "No files received." });
			}

			foreach (var file in files)
			{
				if (file.Length > 0)
				{
					try
					{
						// Tạo tên tệp duy nhất
						var fileName = Path.GetFileNameWithoutExtension(file.FileName);
						var fileExtension = Path.GetExtension(file.FileName);
						var uniqueFileName = $"{fileName}_{DateTime.Now:yyyyMMddHHmmssfff}{fileExtension}";

						// Đường dẫn lưu tệp vào thư mục Datas/Image
						var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "Datas", "Images", uniqueFileName);

						// Kiểm tra và tạo thư mục nếu chưa tồn tại
						var directory = Path.GetDirectoryName(fullPath);
						if (!Directory.Exists(directory))
						{
							Directory.CreateDirectory(directory);
						}

						// Lưu tệp vào đường dẫn
						using (var stream = new FileStream(fullPath, FileMode.Create))
						{
							await file.CopyToAsync(stream);
						}

						// Thêm tên tệp vào danh sách kết quả
						savedFileNames.Add(uniqueFileName);
					}
					catch (Exception ex)
					{
						// Ghi log lỗi tại đây nếu cần
						return StatusCode(500, new { success = false, message = $"Error uploading file: {ex.Message}" });
					}
				}
			}

			return Ok(savedFileNames);
		}

		[HttpPost]
        [Route("VersionImages")]
        public async Task<IActionResult> UploadVersionFiles([FromForm] List<IFormFile> files)
        {
            var savedFileNames = new List<string>();

            if (files == null || files.Count == 0)
            {
                return BadRequest(new { success = false, message = "No files received." });
            }

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    try
                    {
                        // Tạo tên tệp duy nhất
                        var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                        var fileExtension = Path.GetExtension(file.FileName);
                        var uniqueFileName = $"{fileName}_{DateTime.Now:yyyyMMddHHmmssfff}{fileExtension}";

                        // Đường dẫn lưu tệp vào thư mục Datas/Image
                        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "Datas", "VersionImages", uniqueFileName);

                        // Kiểm tra và tạo thư mục nếu chưa tồn tại
                        var directory = Path.GetDirectoryName(fullPath);
                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }

                        // Lưu tệp vào đường dẫn
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        // Thêm tên tệp vào danh sách kết quả
                        savedFileNames.Add(uniqueFileName);
                    }
                    catch (Exception ex)
                    {
                        // Ghi log lỗi tại đây nếu cần
                        return StatusCode(500, new { success = false, message = $"Error uploading file: {ex.Message}" });
                    }
                }
            }

            return Ok(savedFileNames);
        }

		[HttpPost]
		[Route("LibraryImages")]
		public async Task<IActionResult> UploadLibraryFiles([FromForm] List<IFormFile> files)
		{
			var savedFileNames = new List<string>();

			if (files == null || files.Count == 0)
			{
				return BadRequest(new { success = false, message = "No files received." });
			}

			foreach (var file in files)
			{
				if (file.Length > 0)
				{
					try
					{
						// Tạo tên tệp duy nhất
						var fileName = Path.GetFileNameWithoutExtension(file.FileName);
						var fileExtension = Path.GetExtension(file.FileName);
						var uniqueFileName = $"{fileName}_{DateTime.Now:yyyyMMddHHmmssfff}{fileExtension}";

						// Đường dẫn lưu tệp vào thư mục Datas/Image
						var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "Datas", "LibraryImages", uniqueFileName);

						// Kiểm tra và tạo thư mục nếu chưa tồn tại
						var directory = Path.GetDirectoryName(fullPath);
						if (!Directory.Exists(directory))
						{
							Directory.CreateDirectory(directory);
						}

						// Lưu tệp vào đường dẫn
						using (var stream = new FileStream(fullPath, FileMode.Create))
						{
							await file.CopyToAsync(stream);
						}

						// Thêm tên tệp vào danh sách kết quả
						savedFileNames.Add(uniqueFileName);
					}
					catch (Exception ex)
					{
						// Ghi log lỗi tại đây nếu cần
						return StatusCode(500, new { success = false, message = $"Error uploading file: {ex.Message}" });
					}
				}
			}

			return Ok(savedFileNames);
		}
	}
}
