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
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeService employeeService, ILogger<EmployeeController> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
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
                _logger.LogError(ex, "An error occurred while fetching all employees.");
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
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
                _logger.LogError(ex, $"An error occurred while fetching employee with ID {id}.");
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
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
                _logger.LogError(ex, "An error occurred while adding the employee.");
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
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
                _logger.LogError(ex, $"An error occurred while updating employee with ID {id}.");
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
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
                _logger.LogError(ex, $"An error occurred while deleting employee with ID {id}.");
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }
    }
}
