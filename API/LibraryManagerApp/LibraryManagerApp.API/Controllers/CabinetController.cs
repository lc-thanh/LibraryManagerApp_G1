using LibraryManagerApp.Data.Dto;
using LibraryManagerApp.Data.Interfaces;
using LibraryManagerApp.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LibraryManagerApp.API.Controllers
{
    [Route("api/v1/[controller]s")]
    [ApiController]
    public class CabinetController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CabinetController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCabinetById(Guid id)
        {
            var cabinet = await _unitOfWork.CabinetRepository.GetByIdAsync(id);
            if (cabinet == null)
            {
                return NotFound("Cabinet not found.");
            }
            return Ok(cabinet);
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
            };

            _unitOfWork.CabinetRepository.Add(cabinetToCreate);

            var saved = await _unitOfWork.SaveChangesAsync();
            if (saved > 0)
            {
                return Ok("New cabinet created successfully");
            }

            return BadRequest("Failed to create new cabinet");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCabinet(Guid id, CabinetCreateModel cabinetDto)
        {
            if (cabinetDto == null)
            {
                return BadRequest("Cabinet data must be provided.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cabinetToUpdate = await _unitOfWork.CabinetRepository.GetByIdAsync(id);
            if (cabinetToUpdate == null)
            {
                return NotFound("Cabinet not found.");
            }

            cabinetToUpdate.Name = cabinetDto.Name;
            cabinetToUpdate.Location = cabinetDto.Location;

            _unitOfWork.CabinetRepository.Update(cabinetToUpdate);

            var saved = await _unitOfWork.SaveChangesAsync();
            if (saved > 0)
            {
                return Ok("Cabinet updated successfully");
            }

            return BadRequest("Failed to update cabinet");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCabinet(Guid id)
        {
            var cabinetToDelete = await _unitOfWork.CabinetRepository.GetByIdAsync(id);
            if (cabinetToDelete == null)
            {
                return NotFound("Cabinet not found.");
            }

            _unitOfWork.CabinetRepository.Delete(cabinetToDelete);

            var saved = await _unitOfWork.SaveChangesAsync();
            if (saved > 0)
            {
                return Ok("Cabinet deleted successfully");
            }

            return BadRequest("Failed to delete cabinet");
        }
    }
}
