using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILogger<ImageController> _logger;

        public ImageController(IWebHostEnvironment hostingEnvironment, ILogger<ImageController> logger)
        {
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        [HttpGet("GetAllImages")]
        public IActionResult GetAllImages()
        {
            var result = new AllImagesResult
            {
                Images = GetAllImagesFromFolder("Images"),
                VersionImages = GetAllImagesFromFolder("LibraryImages"),
                LibraryImages = GetAllImagesFromFolder("LibraryImages")
            };

            return Ok(result);
        }

        private List<string> GetAllImagesFromFolder(string folderName)
        {
            // Kiểm tra và ghi nhật ký các giá trị null
            if (_hostingEnvironment.WebRootPath == null)
            {
                _logger.LogError("WebRootPath is null.");
                throw new ArgumentNullException(nameof(_hostingEnvironment.WebRootPath), "WebRootPath cannot be null.");
            }

            if (folderName == null)
            {
                _logger.LogError("folderName is null.");
                throw new ArgumentNullException(nameof(folderName), "folderName cannot be null.");
            }

            var imageFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Datas", folderName);
            if (!Directory.Exists(imageFolder))
            {
                return new List<string>();
            }

            var images = Directory.GetFiles(imageFolder).Select(Path.GetFileName).ToList();
            var imageUrls = images.Select(img => Url.Content($"~/Datas/{folderName}/{img}")).ToList();
            return imageUrls;
        }

    }
}
public class AllImagesResult
{
    public List<string> Images { get; set; }
    public List<string> VersionImages { get; set; }
    public List<string> LibraryImages { get; set; }
}
