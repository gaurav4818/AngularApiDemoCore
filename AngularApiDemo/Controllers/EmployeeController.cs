using AngularApiDemo.Models;
using AngularApiDemo.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository employeeRepository;
        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }
        [HttpPost("AddEmployee")]
        public async Task<IActionResult> AddEmployee([FromBody] Employee emp)
        {
            var result = await employeeRepository.Create(emp);
            if (result)
            {
                return Ok(emp);
            }
            return BadRequest("Error in Adding Employee");
        }
        [HttpGet()]
        public async Task<ActionResult> GetEmployees(int? id)
        {

            var result = await employeeRepository.GetEmployee(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("Data Not found");
        }
        [HttpGet("GetOneEmp")]
        public async Task<ActionResult> GetOneEmp(int id)
        {

            var result = await employeeRepository.GetOneEmployee(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("Data Not found");
        }
        [HttpPut("EditEmployee/{id}")]
        public async Task<IActionResult> EditEmployee(int id, [FromBody] Employee emp)
        {
            if (id == emp.Id)
            {
                await employeeRepository.Edit(emp);
                return Ok(emp);

            }
            return BadRequest("Error in Updation of Employee");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var result = await employeeRepository.Delete(id);
            if (result)
            {
                return Ok("Employee Delete Successfully");
            }
            return NotFound("Employee Can not Deleted");
        }
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUpUser(User user)
        {
            var userexist = await employeeRepository.FindUserByEmail(user.Email);
            if(userexist!=null)
            {
                return Conflict("Email already Exists");
            }
            var result = await employeeRepository.SignUp(user);
            if (result)
            {
                return Ok("Signup Successfully");
            }
            return BadRequest("Error in Signup");

        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(User user)
        {
            var userexist = await employeeRepository.FindUserByEmail(user.Email);
            if(userexist==null)
            {
                return Conflict("User Does Not Find!");
            }
            var result = await employeeRepository.Login(user);
            if(result==null)
            {
                return BadRequest("Invalid Password");
            }
            if (result.Token!=null)
            {
                return Ok(result);
            }
            return BadRequest("Error in Login");

        }
    }
}
