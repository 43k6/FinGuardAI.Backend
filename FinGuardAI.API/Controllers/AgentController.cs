using Microsoft.AspNetCore.Mvc;
using API_AI_Agent.Component;
using FinGuardAI.Business.AIAgentComponents;
using FinGuardAI.Business.DTOs;

namespace FinGuardAI.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class AgentController : ControllerBase
    {
        private readonly AgentService _agentService;
        private readonly IngestionService _ingestionService;
        public AgentController(AgentService agentService, IngestionService ingestionService)
        {
            _agentService = agentService;
            _ingestionService = ingestionService;
        }

        [HttpPost("chat")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChatWithAgent([FromBody] ChatRequestDto request)
        {
            if (string.IsNullOrEmpty(request.Message))
                return BadRequest(new { error = "Message required" });

            try
            {
                var answer = await _agentService.RunAgentAsync(request.SessionId ?? "default", request.Message);

                if (string.IsNullOrWhiteSpace(answer))
                    return Ok(new { answer = "I apologize, but I couldn't generate a proper response." });

                return Ok(new { answer });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        [HttpPost("ingest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UploadKnowledgeBase([FromForm] FileUploadRequestDto request)
        {
            var file = request.File;

            if (file == null || file.Length == 0 ||
                (!file.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase) &&
                 !file.FileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase)))
            {
                return BadRequest(new { error = "Only PDF and TXT files are allowed." });
            }


            string tempPath = await Utility.ConvertingTempPathName(file);

            try
            {
                await _ingestionService.IngestPdfAsync(tempPath);

                return Ok(new { ok = true, message = "File uploaded and processed successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to ingest data", details = ex.Message });
            }
            finally
            {
                if (System.IO.File.Exists(tempPath)) System.IO.File.Delete(tempPath);
            }
        }
    }
}