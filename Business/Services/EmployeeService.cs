using Common.Models;
using DataAccess.Repositories;
using System.ComponentModel.DataAnnotations;

namespace Business.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public ApiResponse<IEnumerable<Employee>> GetAllEmployees()
        {
            try
            {
                var employees = _employeeRepository.GetAllEmployees();
                return new ApiResponse<IEnumerable<Employee>>(employees);
            }
            catch (Exception ex)
            {
                return HandleError<IEnumerable<Employee>>(ex);
            }
        }

        public ApiResponse<Employee> GetEmployeeById(int id)
        {
            try
            {
                var employee = _employeeRepository.GetEmployeeById(id);
                if (employee == null)
                {
                    throw new KeyNotFoundException($"Employee with ID {id} not found.");
                }
                return new ApiResponse<Employee>(employee);
            }
            catch (Exception ex)
            {
                return HandleError<Employee>(ex);
            }
        }

        public ApiResponse<Employee> AddEmployee(Employee employee)
        {
            try
            {
                ValidateEmployee(employee);
                _employeeRepository.AddEmployee(employee);
                return new ApiResponse<Employee>(employee, "Employee added successfully.");
            }
            catch (Exception ex)
            {
                return HandleError<Employee>(ex);
            }
        }

        public ApiResponse<Employee> UpdateEmployee(Employee employee)
        {
            try
            {
                ValidateEmployee(employee);
                _employeeRepository.UpdateEmployee(employee);
                return new ApiResponse<Employee>(employee, "Employee updated successfully.");
            }
            catch (Exception ex)
            {
                return HandleError<Employee>(ex);
            }
        }

        public ApiResponse<bool> DeleteEmployee(int id)
        {
            try
            {
                _employeeRepository.DeleteEmployee(id);
                return new ApiResponse<bool>(true, "Employee deleted successfully.");
            }
            catch (Exception ex)
            {
                return HandleError<bool>(ex);
            }
        }

        private void ValidateEmployee(Employee employee)
        {
            var validationContext = new ValidationContext(employee);
            var validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(employee, validationContext, validationResults, true);

            if (!isValid)
            {
                var errorMessages = new List<string>();
                foreach (var validationResult in validationResults)
                {
                    errorMessages.Add(validationResult.ErrorMessage);
                }

                throw new ArgumentException($"Validation failed for employee: {string.Join("; ", errorMessages)}");
            }
        }

        private ApiResponse<T> HandleError<T>(Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error: {ex.Message}");
            // Return an error response
            return new ApiResponse<T>(default, ex.Message, StatusCode.Error);
        }
    }
}
