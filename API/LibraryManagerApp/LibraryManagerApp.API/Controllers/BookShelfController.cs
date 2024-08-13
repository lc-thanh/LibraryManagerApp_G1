using LibraryManagerApp.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Writers;
using Microsoft.VisualBasic;

namespace LibraryManagerApp.API.Controllers
{
    [Route("api/v1/Bookshelves")]
    [ApiController]
    public class BookShelfController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookShelfController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetBookShelves()
        {
            var bookshelves = await _unitOfWork.BookShelfRepository.GetAllAsync();

            return Ok(bookshelves);
        }
    }
}
