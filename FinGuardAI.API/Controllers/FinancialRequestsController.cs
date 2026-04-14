using AutoMapper;
using FinGuardAI.Business.Services;
using FinGuardAI.DataAccess.DTOs;
using FinGuardAI.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static FinGuardAI.DataAccess.DTOs.FinancialResponseDTO;

namespace FinGuardAI.API.Controllers
{
    [Route("api/FinancialRequests")]
    [ApiController]
    public class FinancialRequestsController : ControllerBase
    {
        private readonly FinancialRequestService _financialRequestService;
        private readonly IMapper _mapper;

        public FinancialRequestsController(FinancialRequestService financialRequestService, IMapper mapper)
        {
            _financialRequestService = financialRequestService;
            _mapper = mapper;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<FinancialRequestDto>>> GetAll()
        {
            var requests = await _financialRequestService.GetAll();

            if (requests == null || !requests.Any())
            {
                return NotFound("No Financial Requests Found!");
            }

            var requestsDto = _mapper.Map<IEnumerable<FinancialRequestDto>>(requests);
            return Ok(requestsDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FinancialRequestDto>> GetById(int id)
        {
            var request = await _financialRequestService.GetByID(id);

            if (request == null)
            {
                return NotFound($"Financial Request with ID {id} not found.");
            }

            return Ok(_mapper.Map<FinancialRequestDto>(request));
        }

        [HttpPost("Add")]
        public async Task<ActionResult> Add([FromBody] FinancialRequestDto requestDto)
        {
            if (requestDto == null) return BadRequest();

            // تحويل الـ DTO إلى Entity
            var requestEntity = _mapper.Map<FinancialRequest>(requestDto);

            // --- إسناد الـ User ID يدوياً ---
            // تأكد أن رقم 1 موجود في جدول Users
            requestEntity.CreatedBy = 1;

            requestEntity.CreatedAt = DateTime.Now;

            var result = await _financialRequestService.AddNew(requestEntity);

            if (!result)
                return StatusCode(500, "Error saving request.");

            return CreatedAtAction(nameof(GetById), new { id = requestEntity.Id }, _mapper.Map<FinancialRequestDto>(requestEntity));
        }
    
        [HttpPut("Update")]
        public async Task<ActionResult> Update([FromBody] FinancialRequestDto requestDto)
        {
            if (requestDto.Id <= 0)
            {
                return BadRequest("A valid Id is required in the request body.");
            }

            var existingRequest = await _financialRequestService.GetByID(requestDto.Id);

            if (existingRequest == null)
            {
                return NotFound($"Financial Request with ID {requestDto.Id} not found.");
            }

            // نقل البيانات من الـ DTO إلى الـ Entity
            _mapper.Map(requestDto, existingRequest);

            var success = await _financialRequestService.Update(existingRequest);

            if (!success)
            {
                return StatusCode(500, "An error occurred while updating the financial request.");
            }

            return Ok(new { message = "Updated successfully", id = requestDto.Id });
        }

        [HttpDelete("Delete")]
        public async Task<ActionResult> Delete(int id)
        {
            var success = await _financialRequestService.Delete(id);

            if (!success) return NotFound($"Request with ID {id} not found.");

            return Ok(new { message = "Deleted successfully", id = id });
        }
    }
}