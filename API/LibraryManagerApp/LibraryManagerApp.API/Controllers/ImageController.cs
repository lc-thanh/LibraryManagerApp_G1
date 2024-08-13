using Microsoft.AspNetCore.Mvc;

namespace LibraryManagerApp.API.Controllers
{
    [Route("api/v1/[controller]s")]
    [ApiController]
    public class ImageController : Controller
    {
        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File không hợp lệ.");
            }

            // Đường dẫn lưu trữ hình ảnh
            var filePath = Path.Combine("wwwroot/images/books", file.FileName);

            // Lưu file vào thư mục đã tạo
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Trả về URL đầy đủ của hình ảnh
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var fullPath = $"{baseUrl}/images/books/{file.FileName}";

            return Ok(new { Path = fullPath });
        }
    }
}
