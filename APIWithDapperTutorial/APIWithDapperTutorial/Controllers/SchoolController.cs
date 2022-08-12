using APIWithDapperTutorial.Data.Entities;
using APIWithDapperTutorial.Domain.Interface;
using APIWithDapperTutorial.Domain.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIWithDapperTutorial.Controllers
{
    [Route("api/schools")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        private readonly ISchoolRepostory _schoolRepository;
        public SchoolController(ISchoolRepostory schoolRepostory)
        {
            _schoolRepository = schoolRepostory;
        }

        [HttpGet]
        public async Task<IActionResult> GetSchoolsAsync()
        {
            var schools= await this._schoolRepository.GetAllSchoolsAsync();

            return Ok(schools);

        }

        [HttpGet("{id}",Name = "schoolById")]
        //[HttpGet]
        //[Route("schoolById/{id}")]
        public async Task<IActionResult> GetschoolById([FromRoute]int id)
        {
            var school = await this._schoolRepository.GetSchoolByIdAsync(id);

            return Ok(school);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSchool(SchoolDto schoolDto)
        {
            try
            {
                var school = await this._schoolRepository.CreateSchoolAsync(schoolDto);
                return CreatedAtRoute("SchoolById", new { id = school.Id }, school);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSchool(int id, SchoolDto school)
        {
            try
            {
                var dbSchool = await _schoolRepository.GetSchoolByIdAsync(id);
                if (dbSchool == null)
                    return NotFound();

                await _schoolRepository.UpdateSchoolAsync(id, school);
                return NoContent();
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchool(int id)
        {
            try
            {
                var dbSchool = await this._schoolRepository.GetSchoolByIdAsync(id);
                if (dbSchool == null)
                    return NotFound();

                await _schoolRepository.DeleteSchoolByIdAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("ByStudentId/{id}")]
        public async Task<IActionResult> GetSchoolByStudentId(int id)
        {
            try
            {
                var school = await _schoolRepository.GetSchoolByStudentIdAsync(id);
                if (school == null)
                    return NotFound();

                return Ok(school);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}/MultipleResult")]
        public async Task<IActionResult> GetSchoolStudentsBySchoolId(int id)
        {
            try
            {
                var school = await _schoolRepository.GetSchoolWithStudentsBySchoolId(id);
                if (school == null)
                    return NotFound();

                return Ok(school);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("MultipleMapping")]
        public async Task<IActionResult> GetSChoolsStudentssMultipleMapping()
        {
            try
            {
                var schools = await _schoolRepository.GetMultipleSchoolsAndStudentsAsyn();

                return Ok(schools);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("multiple")]
        public async Task<IActionResult> CreateCompany(List<SchoolDto> schools)
        {
            try
            {
                await _schoolRepository.CreateListOfSchoolsByAsync(schools);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
    }
}
