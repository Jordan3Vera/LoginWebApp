using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiRest.Data;
using WebApiRest.Helper;
using WebApiRest.Model;
using WebApiRest.Model.DTOs;

namespace WebApiRest.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly WebApiRestContext _context;

        public EmployeesController(WebApiRestContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public IEnumerable<EmployeeDTO> GetEmployee()
        {
            return Mapper.Map<IEnumerable<EmployeeDTO>>(_context.Employee.OrderBy(x => x.LastName).ThenBy(x => x.FirstName));
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeId([FromRoute] int id)
        {
            var employee = await _context.Employee.SingleOrDefaultAsync(m => m.Id == id);

            if (employee == null)
            {
                return BadRequest(ErrorHelper.Response(404,"Registro no encontrado"));
            }

            return Ok(Mapper.Map<EmployeeDTO>(employee));
        }


        //Login para el empleado
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] dynamic credentials)
        {
            var username = (string)credentials["username"];
            var password = (string)credentials["password"];

            var employee = await _context.Employee.SingleOrDefaultAsync(m => m.Username == username && m.Password == password);

            if (employee == null)
            {
                return BadRequest(ErrorHelper.Response(404,"Registro no encontrado"));
            }

            return Ok(Mapper.Map<EmployeeDTO>(employee));
        }

        // PUT: api/Employees/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, [FromBody] EmployeeDTO employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }

            if (id != employee.Id)
            {
                return BadRequest();
            }

            _context.Entry(Mapper.Map<Employee>(employee)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return BadRequest(ErrorHelper.Response(404,"Registro no encontrado"));
                }
                else
                {
                    throw;
                }
            }

            return BadRequest(ErrorHelper.Response(400,"No se pudo actualziar el registro"));
        }

        // POST: api/Employees
        [HttpPost]
        public async Task<IActionResult> PostEmployee([FromBody] EmployeeDTO employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }

            var e = Mapper.Map<Employee>(employee);
            _context.Employee.Add(e);
            await _context.SaveChangesAsync();
            employee.Id = e.Id;

            return CreatedAtAction(nameof(GetEmployee), 
                new 
                { 
                    id = e.Id,
                    username = e.Username,
                    firstname = e.FirstName,
                    lastname = e.LastName
                }, employee);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }

            var employee = await _context.Employee.SingleOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return BadRequest(ErrorHelper.Response(404,"Registro no encontrado"));
            }

            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();

            return Ok(Mapper.Map<EmployeeDTO>(employee));
        }


        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.Id == id);
        }
    }
}
