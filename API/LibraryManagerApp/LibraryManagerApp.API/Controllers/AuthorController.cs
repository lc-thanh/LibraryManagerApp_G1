using LibraryManagerApp.Data.Dto;
using LibraryManagerApp.Data.Interfaces;
using LibraryManagerApp.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagerApp.API.Controllers
{
    [Route("api/v1/[controller]s")]
    [ApiController]
    public class AuthorsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthorsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAuthors()
        {
            var authors = await _unitOfWork.AuthorRepository.GetAllAsync();
            return Ok(authors);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuthor(AuthorCreateModel authorDto)
        {
            if (authorDto == null)
            {
                return BadRequest("Author data must be provided.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var authorToCreate = new Author
            {
                Name = authorDto.Name,
                Books = new List<Book>() 
            };

            _unitOfWork.AuthorRepository.Add(authorToCreate);

            var saved = await _unitOfWork.SaveChangesAsync();
            if (saved > 0)
            {
                return Ok("New author created successfully");
            }

            return BadRequest("Failed to create new author");
        }
    }
}
