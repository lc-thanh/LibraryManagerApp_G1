using LibraryManagerApp.Data.Dto;
using LibraryManagerApp.Data.Interfaces;
using LibraryManagerApp.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryManagerApp.API.Controllers
{
    [Route("api/v1/[controller]s")]
    [ApiController]
    public class LoanController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public LoanController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize(Roles = "Librarian")]
        [HttpPost]
        public async Task<IActionResult> CreateLoan(LoanCreateModel loanDto)
        {
            if(loanDto == null || !loanDto.BookIds.Any())
            {
                return BadRequest("Invalid loan data.");
            }

            // Find member
            var member = await _unitOfWork.UserRepository.GetByEmailAsync(loanDto.MemberEmail);
            if (member == null)
                return NotFound("Cannot find user with provided email");

            // Get Librarian created this Loan
            string librarianEmail = User.FindFirst(ClaimTypes.Name)?.Value;
            var librarian = await _unitOfWork.UserRepository.GetByEmailAsync(librarianEmail);

            var loan = new Loan
            {
                MemberId = member.Id,
                LibrarianId = librarian.Id,
                LoanDate = loanDto.LoanDate,
                DueDate = loanDto.DueDate,
            };

            foreach (var bookId in loanDto.BookIds)
            {
                var book = await _unitOfWork.BookRepository.GetByIdAsync(bookId);
                if (book == null)
                    { continue; }
                if (book.AvailableQuantity == 0)
                    { continue; }

                var loanDetail = new LoanDetail
                {
                    Book = book,
                    Loan = loan,
                    Quantity = 1
                };

                book.AvailableQuantity--;
                _unitOfWork.BookRepository.Update(book);
                _unitOfWork.LoanDetailRepository.Add(loanDetail);
                //if (await _unitOfWork.SaveChangesAsync() == 0)
                //{
                //    return BadRequest("Something wrong while create loanDetails!");
                //}
            }

            _unitOfWork.LoanRepository.Add(loan);
            var saved = await _unitOfWork.SaveChangesAsync();
            if (saved > 0)
            {
                return Ok("Created new loan successfully!");
            }
            return BadRequest("Something wrong while create loan!");
        }

        [HttpPost("return/{loanId}")]
        public async Task<IActionResult> ReturnBooks(Guid loanId)
        {
            await _unitOfWork.LoanRepository.ReturnBooksAsync(loanId);

            return Ok("Books returned successfully.");
        }


    }
}
