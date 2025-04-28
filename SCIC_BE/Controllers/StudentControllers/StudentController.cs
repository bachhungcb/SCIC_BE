using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCIC_BE.Interfaces.IServices;


namespace SCIC_BE.Controllers.StudentControllers
{
    [Route("/api/v1/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetListStudentAsync()
        {
            var studentList = await _studentService.GetListStudentAsync();
            if (studentList == null || studentList.Count == 0)
            {
                return NotFound("No students found");
            }

            return Ok(studentList);
        }


    }
}
