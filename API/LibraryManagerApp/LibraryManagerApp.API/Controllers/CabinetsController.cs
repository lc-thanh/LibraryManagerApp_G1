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
        public async Task<IActionResult> GetAllCabinets()
        {
            var cabinets = await _unitOfWork.CabinetRepository.GetAllAsync();
            return Ok(cabinets);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCabinet(CabinetCreateModel cabinetDto)
        {
            if (cabinetDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cabinetToCreate = new Cabinet
            {
                Name = cabinetDto.Name,
                Location = cabinetDto.Location,
                Description = cabinetDto.Description, 
            };

            _unitOfWork.CabinetRepository.Add(cabinetToCreate);

            var saved = await _unitOfWork.SaveChangesAsync();
            if (saved > 0)
            {
                return Ok("New cabinet created successfully");
            }

            return BadRequest("Failed to create new cabinet");
        }
    }
}

