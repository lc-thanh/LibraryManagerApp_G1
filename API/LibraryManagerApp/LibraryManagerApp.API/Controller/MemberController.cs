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
    public class MemberController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public MemberController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMember()
        {
            var members = await _unitOfWork.MemberRepository.GetAllAsync();
            return Ok(members);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMember(MemberCreateModel memberDto)
        {
            if (memberDto == null)
            {
                return BadRequest("Member data must be provided.");
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
                Address = memberDto.Address,
                DateOfBirth = memberDto.DateOfBirth,
                Password = memberDto.Password,
                Role = memberDto.Role,
                MembershipDate = memberDto.MembershipDate,
            };

            _unitOfWork.MemberRepository.Add(memberToCreate);

            var saved = await _unitOfWork.SaveChangesAsync();
            if (saved > 0)
            {
                return Ok("New member created successfully");
            }
            return BadRequest("Failed to create new member");
        }

        [HttpPut{("Id")}]
        public async Task<IActionResult> UpdateMember(Guid Id, MemberCreateModel memberDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var memberToUpdate = await _unitOfWork.MemberRepository.GetAllAsync(Id);
        if (memberToUpdate == null)
        {
            return NotFound("Member not found!");
        }
        memberToUpdate.FullName = memberDto.FullName;
        memberToUpdate.Email = memberDto.Email;
        memberToUpdate.Phone = memberDto.Phone;
        memberToUpdate.Address = memberDto.Address;
        memberToUpdate.DateOfBirth = memberDto.DateOfBirth;
        memberToUpdate.Password = memberDto.Password;
        memberToUpdate.Role = memberDto.Role;
        memberToUpdate.MembershipDate = memberDto.MembershipDate;

        _unitOfWork.MemberRepository.Update(Member);
        var saved = await _unitOfWork.SaveChangesAsync();
        if (saved > 0)
        {
            return Ok("Member update successfully!");
        }
        return StatusCode(500, "Failes to update member");
    }
    [HttpDelete{("Id")}]
        public async Task<IActionResult> DeleteMember(Guid Id)
{
    var memberToDelete = await _unitOfWork.MemberRepository.GetAllAsync(Id);
    if (memberToDelete == null)
    {
        return NotFound("Member not found!");
    }
    _unitOfWork.MemberRepository.Delete(memberToDelete);
    var saved = await _unitOfWork.SaveChangesAsync();
    if (saved > 0)
    {
        return Ok("Member deleted successfully");
    }
    return StatusCode(500, "Failed to delete member");
}
[HttpGet("search")]
public async Task<IActionResult> SearchMember(string query)
{
    if (string.IsNullOrWhiteSpace(query))
    {
        return BadRequest("Search query is required.");
    }
    var members = await _unitOfWork.MemberRepository.SearchAsync(query);
    return Ok(members);
}

    }
}

        