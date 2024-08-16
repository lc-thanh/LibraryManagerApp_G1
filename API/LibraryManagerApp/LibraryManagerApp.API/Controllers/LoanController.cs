using LibraryManagerApp.Data.Dto;
using LibraryManagerApp.Data.Enum;
using LibraryManagerApp.Data.Interfaces;
using LibraryManagerApp.Data.Models;
using LibraryManagerApp.Data.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
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
        [HttpGet]
        public async Task<IActionResult> GetLoans(
            [FromQuery] string? memberEmail,
            [FromQuery] string? timePeriod,
            [FromQuery] StatusEnum? status,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var loansQuery = _unitOfWork.LoanRepository.GetAllInforsQuery();

            var loanViewModels = loansQuery.Select(l => new LoanViewModel
            {
                Id = l.Id,
                LoanCode = l.LoanCode,
                LoanDate = l.LoanDate,
                DueDate = l.DueDate,
                ReturnedDate = l.ReturnedDate,
                Status = l.Status,
                MemberId = l.MemberId,
                MemberEmail = l.Member.Email,
                MemberFullName = l.Member.FullName,
                LibrarianId = l.LibrarianId,
                LibrarianFullName = l.Librarian.FullName,
                BookNames = l.LoanDetails.Select(ld => ld.Book.Title).ToList()
            });


            List<Expression<Func<LoanViewModel, bool>>> filterList = new List<Expression<Func<LoanViewModel, bool>>>();
            if (!string.IsNullOrEmpty(memberEmail))
            {
                filterList.Add(l => l.MemberEmail.Equals(memberEmail));
            }
            if (!string.IsNullOrEmpty(timePeriod))
            {
                DateTime.TryParse(timePeriod.Split('-')[0], out DateTime dateStart);

                DateTime.TryParse(timePeriod.Split('-')[1], out DateTime dateEnd);

                filterList.Add(l => dateStart <= l.LoanDate && l.LoanDate <= dateEnd);
            }
            if (status != null)
            {
                filterList.Add(l => l.Status == status);
            }


            Func<IQueryable<LoanViewModel>, IOrderedQueryable<LoanViewModel>>? orderFunc = null;

            // Mặc định là xếp theo LoanDate mới nhất
            orderFunc = query => query.OrderByDescending(l => l.LoanDate);


            PaginatedResult<LoanViewModel> paginatedLoans = await _unitOfWork.BaseRepository<LoanViewModel>().GetPaginatedAsync(
                loanViewModels,
                filterList,
                orderFunc,
                "",
                pageNumber,
                pageSize
            );

            return Ok(paginatedLoans);
        }

        [Authorize(Roles = "Librarian")]
        [HttpPost]
        public async Task<IActionResult> CreateLoan([FromBody] LoanCreateModel loanDto)
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

            string loanCode = _unitOfWork.LoanRepository.GenerateLoanCode();
            while (await _unitOfWork.LoanRepository.FindByCodeAsync(loanCode) != null)
            {
                loanCode = _unitOfWork.LoanRepository.GenerateLoanCode();
            }

            var loan = new Loan
            {
                MemberId = member.Id,
                LibrarianId = librarian.Id,
                LoanDate = loanDto.LoanDate,
                DueDate = loanDto.DueDate,
                LoanCode = loanCode,
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
