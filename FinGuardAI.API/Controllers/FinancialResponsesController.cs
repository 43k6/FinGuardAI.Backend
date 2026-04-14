using AutoMapper;
using FinGuardAI.Business.Services;
using FinGuardAI.DataAccess.DTOs;
using FinGuardAI.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using static FinGuardAI.DataAccess.DTOs.FinancialResponseDTO;

namespace FinGuardAI.API.Controllers
{
    [Route("api/FinancialResponses")]
    [ApiController]
    public class FinancialResponsesController : ControllerBase
    {
        private readonly FinancialResponseService _responseService;
        private readonly IMapper _mapper;

        public FinancialResponsesController(FinancialResponseService responseService, IMapper mapper)
        {
            _responseService = responseService;
            _mapper = mapper;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<FinancialResponseDto>>> GetAll()
        {
            var responses = await _responseService.GetAll();
            if (responses == null || !responses.Any())
            {
                return NotFound("No Financial Responses Found!");
            }

            return Ok(_mapper.Map<IEnumerable<FinancialResponseDto>>(responses));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FinancialResponseDto>> GetById(int id)
        {
            var response = await _responseService.GetByID(id);
            if (response == null)
            {
                return NotFound($"Response with ID {id} not found.");
            }

            return Ok(_mapper.Map<FinancialResponseDto>(response));
        }

        [HttpPost("Add")]
        public async Task<ActionResult> Add([FromBody] FinancialResponseDto responseDto)
        {
            if (responseDto == null) return BadRequest();

            // 1. تحويل الـ DTO إلى Entity
            var responseEntity = _mapper.Map<FinancialResponse>(responseDto);

            // 2. إسناد المعرفات يدوياً
            // هنا نسند الـ User ID (مثلاً 1 للتجربة)
            responseEntity.CreatedBy = 1;
            responseEntity.CreatedAt = DateTime.Now;

            // 3. الحفظ
            var result = await _responseService.AddNew(responseEntity);

            if (!result)
                return StatusCode(500, "A problem occurred while saving the response.");

            // 4. العودة بالـ DTO المحدث
            var resultDto = _mapper.Map<FinancialResponseDto>(responseEntity);
            return CreatedAtAction(nameof(GetById), new { id = resultDto.Id }, resultDto);
        }

        [HttpPut("Update")]
        public async Task<ActionResult> Update([FromBody] FinancialResponseDto responseDto)
        {
            if (responseDto.Id <= 0)
                return BadRequest("Invalid ID");

            var existingResponse = await _responseService.GetByID(responseDto.Id);
            if (existingResponse == null) return NotFound();

            _mapper.Map(responseDto, existingResponse);

            var success = await _responseService.Update(existingResponse);
            if (!success) return StatusCode(500, "Update failed.");

            return Ok(new { message = "Updated successfully", id = responseDto.Id });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var success = await _responseService.Delete(id);
            if (!success) return NotFound();

            return Ok(new { message = "Deleted successfully", id = id });
        }
    }
}