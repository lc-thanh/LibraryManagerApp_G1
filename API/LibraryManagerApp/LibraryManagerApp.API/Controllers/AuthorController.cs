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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthorById(Guid id)
        {
            var author = await _unitOfWork.AuthorRepository.GetByIdAsync(id);
            if (author == null)
            {
                return NotFound("Author not found.");
            }
            return Ok(author);
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(Guid id, AuthorCreateModel authorDto)
        {
            if (authorDto == null)
            {
                return BadRequest("Author data must be provided.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var authorToUpdate = await _unitOfWork.AuthorRepository.GetByIdAsync(id);
            if (authorToUpdate == null)
            {
                return NotFound("Author not found.");
            }

            authorToUpdate.Name = authorDto.Name;
           

            _unitOfWork.AuthorRepository.Update(authorToUpdate);

            var saved = await _unitOfWork.SaveChangesAsync();
            if (saved > 0)
            {
                return Ok("Author updated successfully");
            }

            return BadRequest("Failed to update author");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(Guid id)
        {
            var authorToDelete = await _unitOfWork.AuthorRepository.GetByIdAsync(id);
            if (authorToDelete == null)
            {
                return NotFound("Author not found.");
            }

            _unitOfWork.AuthorRepository.Delete(authorToDelete);

            var saved = await _unitOfWork.SaveChangesAsync();
            if (saved > 0)
            {
                return Ok("Author deleted successfully");
            }

            return BadRequest("Failed to delete author");
        }
    }
}
