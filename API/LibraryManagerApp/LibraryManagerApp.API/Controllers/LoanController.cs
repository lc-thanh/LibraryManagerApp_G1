using LibraryManagerApp.Data;
using LibraryManagerApp.Data.Dto;
using LibraryManagerApp.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagerApp.API.Controllers
{
    [Route("api/v1/[controller]s")]
    [ApiController]
    public class LoanController : Controller
    {
        private readonly UnitOfWork _unitOfWork;
        public LoanController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> CreateLoan(LoanCreateModel loanDto)
        {
            if(loanDto == null || !loanDto.BookId.Any())
            {
                return BadRequest("Invalid loan data.");
            }
            var loan = new Loan
            {
                MemberId = loanDto.MemberId,
                LoanDate = loanDto.LoanDate,
                DueDate = loanDto.DueDate,
                LoanDetails = loanDto.BookId.Select(bookId => new LoanDetail { BookId = bookId,Quantity = loanDto.Quantity }).ToList()
            };
            await _unitOfWork.LoanRepository.CreateLoanAsync(loan);
            return Ok("Loan created successfully.");
        }
        [HttpPost("return/{loanId}")]
        public async Task<IActionResult> ReturnBooks(Guid loanId)
        {
            await _unitOfWork.LoanRepository.ReturnBooksAsync(loanId);

            return Ok("Books returned successfully.");
        }


    }
}
