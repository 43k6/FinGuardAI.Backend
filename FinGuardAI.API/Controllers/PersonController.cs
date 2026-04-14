using AutoMapper;
using FinGuardAI.Business.Services;
using FinGuardAI.DataAccess.DTOs;
using FinGuardAI.DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinGuardAI.API.Controllers
{
    [Route("api/People")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly PersonService _personService;
        private readonly IMapper _mapper;

        public PeopleController(PersonService personService, IMapper mapper)
        {
            _personService = personService;
            _mapper = mapper;
        }


        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Person>>> GetAll()
        {

            IEnumerable<Person> StudentsList = await _personService.GetAll();
            if (StudentsList.Count() == 0)
            {
                return NotFound("No Students Found!");
            }
            return Ok(StudentsList); // Returns the list of students

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetById(int id)
        {
            var person = await _personService.GetByID(id);
            if (person == null)
            {
                return NotFound($"Person with ID {id} not found.");
            }
            return Ok(person);
        }



        [HttpPost("Add")]
        public async Task<ActionResult> Add([FromBody] PersonDto personDto)
        {
            if (personDto == null) return BadRequest();

            
            if (await _personService.IsExistByNationalID(personDto.NationalID))
            {
                return BadRequest("National ID already exists.");
            }

            var personEntity = _mapper.Map<Person>(personDto);

            var result = await _personService.AddNew(personEntity);

            if (!result)
                return StatusCode(500, "A problem occurred while handling your request.");

            personDto.PersonID = personEntity.Id;

            return CreatedAtAction(nameof(GetById), new { id = personDto.PersonID }, personDto);
        }

        [HttpPut("Update")]
        public async Task<ActionResult> Update([FromBody] PersonDto personDto)
        {
            // 1. التأكد أن الـ ID موجود داخل الـ DTO المرسل
            if (personDto.PersonID <= 0)
            {
                return BadRequest("A valid PersonID is required in the request body.");
            }

            // 2. البحث عن الشخص في قاعدة البيانات باستخدام الـ ID الموجود في الـ DTO
            var existingPerson = await _personService.GetByID(personDto.PersonID);

            if (existingPerson == null)
            {
                return NotFound($"Person with ID {personDto.PersonID} not found.");
            }

            // 3. نقل البيانات من الـ DTO إلى الكائن الأصلي (Existing Entity)
            // ملاحظة: الـ AutoMapper سيقوم بتحديث الحقول الموجودة في الـ DTO فقط
            _mapper.Map(personDto, existingPerson);

            // 4. تنفيذ التحديث في قاعدة البيانات
            var success = await _personService.Update(existingPerson);

            if (!success)
            {
                return StatusCode(500, "An error occurred while updating the person.");
            }

            return Ok(new { message = "Updated successfully", id = personDto.PersonID });
        }

        [HttpDelete("Delete")]
        public async Task<ActionResult> Delete(int id)
        {
            var success = await _personService.Delete(id);
            if (!success) return NotFound();

            return Ok(new { message = "Deleted successfully", id = id });
        }
    }
}
