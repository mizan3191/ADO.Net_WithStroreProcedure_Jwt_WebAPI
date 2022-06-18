using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeesService _service;

        public EmployeesController(IEmployeesService service)
        {
            _service = service;
        }

        [HttpPost]
        public ActionResult<Employee> AddEmployee(Employee employee)
        {
            _service.AddEmployee(employee);
            return Ok();
        }

        [HttpGet]
        public List<Employee> GetEmployee()
        {
            var result = _service.GetEmployees();
            return result;
        }
        
        [HttpGet("{id}")]
        public List<Employee> GetEmployeeById(int id)
        {
            var result = _service.GetEmployeeById(id);
            return result;
        }
        
        [HttpDelete]
        public IActionResult DeleteEmployeeById(int id)
        {
            _service.DeleteEmployeeById(id);
            return Ok();
        }

        [HttpPut]
        public ActionResult<Employee> UpdateEmployee(Employee employee, int id)
        {
            _service.UpdateEmployee(employee, id);
            return Ok();
        }
    }
}
