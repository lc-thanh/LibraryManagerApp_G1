using LibraryManagerApp.Data.Interfaces;
using LibraryManagerApp.Data.Models;
using Microsoft.AspNetCore.Mvc;
using LibraryManagerApp.Data.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using LibraryManagerApp.Data;

namespace LibraryManagerApp.API.Controllers
{
    [Route("api/v1/[controller]s")]
    [ApiController]
    public class LibrarianController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public LibrarianController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLibrarian()
        {
            var librarians = await _unitOfWork.LibrarianRepository.GetAllAsync();
            return Ok(librarians);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMember(LibrarianCreateModel librarianDto)
        {
            if (librarianDto == null)
            {
                return BadRequest("Member data must be provided.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Librarian librarianToCreate = new Librarian
            {
                FullName = librarianDto.FullName,
                Email = librarianDto.Email,
                Phone = librarianDto.Phone,
                Address = librarianDto.Address,
                DateOfBirth = librarianDto.DateOfBirth,
                Password = librarianDto.Password,
                Role = librarianDto.Role,
            };

            _unitOfWork.LibrarianRepository.Add(librarianToCreate);

            var saved = await _unitOfWork.SaveChangesAsync();
            if (saved > 0)
            {
                return Ok("New librarian created successfully");
            }
            return BadRequest("Failed to create new librarian");
        }

        [HttpPut{("Id")}]
        public async Task<IActionResult> UpdateLibrarian(Guid Id, LibrarianCreateModel librarianDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var librarianToUpdate = await _unitOfWork.MemberRepository.GetAllAsync(Id);
        if (librarianToUpdate == null)
        {
            return NotFound("Member not found!");
        }
        librarianToUpdate.FullName = librarianDto.FullName;
        librarianToUpdate.Email = librarianDto.Email;
        librarianToUpdate.Phone = librarianDto.Phone;
        librarianToUpdate.Address = librarianDto.Address;
        librarianToUpdate.DateOfBirth = librarianDto.DateOfBirth;
        librarianToUpdate.Password = librarianDto.Password;
        librarianToUpdate.Role = librarianDto.Role;

        _unitOfWork.LibrarianRepository.Update(Librarian);
        var saved = await _unitOfWork.SaveChangesAsync();
        if (saved > 0)
        {
            return Ok("Librarian update successfully!");
        }
        return StatusCode(500, "Failes to update librarian");
    }
    [HttpDelete{("Id")}]
        public async Task<IActionResult> DeleteLibrarian(Guid Id)
{
    var librarianToDelete = await _unitOfWork.LibrarianRepository.GetAllAsync(Id);
    if (librarianToDelete == null)
    {
        return NotFound("Member not found!");
    }
    _unitOfWork.LibrarianRepository.Delete(librarianToDelete);
    var saved = await _unitOfWork.SaveChangesAsync();
    if (saved > 0)
    {
        return Ok("Librarian deleted successfully");
    }
    return StatusCode(500, "Failed to delete librarian");
}
[HttpGet("search")]
public async Task<IActionResult> SearchLibrarian(string query)
{
    if (string.IsNullOrWhiteSpace(query))
    {
        return BadRequest("Search query is required.");
    }
    var librarians = await _unitOfWork.LibrarianRepository.SearchAsync(query);
    return Ok(librarians);
}

    }
}
