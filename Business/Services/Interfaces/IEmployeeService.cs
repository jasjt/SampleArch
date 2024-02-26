using Common.Models;

namespace Business.Services
{
    public interface IEmployeeService
    {
        ApiResponse<IEnumerable<Employee>> GetAllEmployees();
        ApiResponse<Employee> GetEmployeeById(int id);
        ApiResponse<Employee> AddEmployee(Employee employee);
        ApiResponse<Employee> UpdateEmployee(Employee employee);
        ApiResponse<bool> DeleteEmployee(int id);
    }
}
