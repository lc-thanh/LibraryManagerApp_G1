using LibraryManagerApp.Data.Dto;
using LibraryManagerApp.Data.Interfaces;
using LibraryManagerApp.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LibraryManagerApp.API.Controllers
{
    [Route("api/v1/[apicontroller]s")]
    [ApiController]
    public class CabinetsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CabinetsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var cabinets = await _unitOfWork.CabinetRepository.GetAllAsync();
            return Ok(cabinets);
        }
        [HttpPost]
        public async Task<IActionResult> CreateMember(MemberCreateModel memberDto)
        {
            if(memberDto == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Member memberToCreate = new Member
            {
                FullName = memberDto.FullName,
                Email = memberDto.Email,
                Phone = memberDto.Phone,
                DateOfBirth = memberDto.DateOfBirth,
                Address = memberDto.Address,
                Password = memberDto.Password,
            };
            _unitOfWork.MemberRepository.Add(memberToCreate);
            var saved = await _unitOfWork.SaveChangesAsync();
            if(saved > 0)
            {
                return Ok("Create new member");
            }
            return BadRequest();
        }
    }
}
