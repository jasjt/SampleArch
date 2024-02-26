using Business.Services;
using Common.Models;
using Microsoft.AspNetCore.Mvc;
using EnumStatusCode = Common.Models.StatusCode;

namespace BackendApis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            try
            {
                var response = _employeeService.GetAllEmployees();
                return Ok(response); // Return 200 OK with ApiResponse object
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Return 500 Internal Server Error
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetEmployeeById(int id)
        {
            try
            {
                var response = _employeeService.GetEmployeeById(id);
                if (response.StatusCode == EnumStatusCode.Success)
                {
                    return Ok(response.Data); // Return 200 OK with employee object
                }
                else
                {
                    return NotFound(); // Return 404 Not Found
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Return 500 Internal Server Error
            }
        }

        [HttpPost]
        public IActionResult AddEmployee(Employee employee)
        {
            try
            {
                var response = _employeeService.AddEmployee(employee);
                return CreatedAtAction(nameof(GetEmployeeById), new { id = response.Data.Id }, response.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Return 500 Internal Server Error
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateEmployee(int id, Employee employee)
        {
            try
            {
                if (id != employee.Id)
                {
                    return BadRequest("ID in the URL does not match the ID in the payload.");
                }

                var response = _employeeService.UpdateEmployee(employee);
                if (response.StatusCode == EnumStatusCode.Success)
                {
                    return NoContent(); // Return 204 No Content
                }
                else
                {
                    return NotFound(); // Return 404 Not Found
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Return 500 Internal Server Error
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            try
            {
                var response = _employeeService.DeleteEmployee(id);
                if (response.StatusCode == EnumStatusCode.Success)
                {
                    return NoContent(); // Return 204 No Content
                }
                else
                {
                    return NotFound(); // Return 404 Not Found
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Return 500 Internal Server Error
            }
        }
    }
}
