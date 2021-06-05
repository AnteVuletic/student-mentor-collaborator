using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentMentor.Domain.Models.ViewModels;
using StudentMentor.Domain.Repositories.Interfaces;
using StudentMentor.Web.Infrastructure;

namespace StudentMentor.Web.Controllers
{
    public class StudentController : ApiController
    {
        private readonly IStudentRepository _studentRepository;


        public StudentController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        [Authorize(Policy = Policies.Admin)]
        [HttpGet]
        public async Task<ActionResult<ICollection<StudentModel>>> GetStudents()
        {
            var students = await _studentRepository.GetStudents();
            return Ok(students);
        }

        [Authorize(Policy = Policies.Admin)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStudent([FromRoute] int studentId)
        {
            var response = await _studentRepository.DeleteStudent(studentId);
            if (response.IsError)
                return BadRequest(response.Message);

            return Ok();
        }
}
}
