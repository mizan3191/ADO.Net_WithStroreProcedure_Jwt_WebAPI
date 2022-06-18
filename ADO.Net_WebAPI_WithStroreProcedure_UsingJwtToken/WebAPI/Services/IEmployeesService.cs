using WebAPI.Models;

namespace WebAPI.Services
{
    public interface IEmployeesService
    {
        List<Employee> GetEmployeeById(int id);
        List<Employee> GetEmployees();
        void AddEmployee(Employee employee);
        void UpdateEmployee(Employee employee, int id);
        void DeleteEmployeeById(int id);
    }
}