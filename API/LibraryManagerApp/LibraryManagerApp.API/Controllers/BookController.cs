using LibraryManagerApp.Data.Interfaces;
using LibraryManagerApp.Data.Models;
using Microsoft.AspNetCore.Mvc;
using LibraryManagerApp.Data.Dto;

namespace LibraryManagerApp.API.Controllers
{
    [Route("api/v1/[controller]s")]
    [ApiController]
    public class BookController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public string Title { get; private set; }
        public Author AuthorName { get; private set; }
        public string Publisher { get; private set; }
        public int? PublishedYear { get; private set; }
        public int Quantity { get; private set; }
        public int TotalPages { get; private set; }
        public string ImageUrl { get; private set; }
        public string Description { get; private set; }

        public BookController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _unitOfWork.BookRepository.GetAllAsync();
            return Ok(books);
        }
        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] BookController bookDto)
        {
            if (bookDto == null)
            {
                return BadRequest("Book data is required.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Book bookToCreate = new Book
            {
                Title = bookDto.Title,
                Author = bookDto.AuthorName,
                Publisher = bookDto.Publisher,
                PublishedYear = bookDto.PublishedYear,
                Quantity = bookDto.Quantity,
                TotalPages = bookDto.TotalPages,
                ImageUrl = bookDto.ImageUrl,
                Description = bookDto.Description,
            };

            _unitOfWork.BookRepository.Add(bookToCreate);

            var saved = await _unitOfWork.SaveChangesAsync();
            if (saved > 0)
            {
                return Ok("Created new book!");
            }
            return BadRequest();

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(Guid id, BookCreateModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existringBook = await _unitOfWork.BookRepository.GetByIdAsync(id);
            if (existringBook == null)
            {
                return NotFound("Book not found.");
            }
            existringBook.Title = model.Title;
            existringBook.Author = model.Author;
            existringBook.Publisher = model.Publisher;
            existringBook.PublishedYear = model.PublishedYear;
            existringBook.Quantity = model.Quantity;
            existringBook.TotalPages = model.TotalPages;
            existringBook.ImageUrl = model.ImageUrl;
            existringBook.Description = model.Description;
            existringBook.Category = model.Category;
            existringBook.BookShelf = model.ShelfName;

            _unitOfWork.BookRepository.Update(existringBook);
            var saved = await _unitOfWork.SaveChangesAsync();
            if(saved > 0)
            {
                return Ok("Book updated successfully.");
            }
            return StatusCode(500, "A problem occurred while updating the book.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            var existingBook = await _unitOfWork.BookRepository.GetByIdAsync(id);
            if (existingBook == null)
            {
                return NotFound("Book not found");
            }
            _unitOfWork.BookRepository.Remove(existingBook);
            var saved = await _unitOfWork.SaveChangesAsync();
            if (saved > 0)
            {
                return Ok("Book deleted seccessfully.");
            }
            return StatusCode(500, "A problem occurred while deleting the book.");
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchBook(string query)
        {
            if(string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Search query is required.");
            }
            var books = await _unitOfWork.BookRepository.SearchAsync(query);
            return Ok(books);
        }
    }
}
