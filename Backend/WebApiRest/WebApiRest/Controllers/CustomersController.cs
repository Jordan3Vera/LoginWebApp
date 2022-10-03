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
    [Route("api/Customers")]
    [ApiController]
    [Authorize]
    public class CustomersController : ControllerBase
    {
        private readonly WebApiRestContext _context;
        public CustomersController(WebApiRestContext context)
        {
            _context = context;
        }

        // GET: api/Customers
        [HttpGet]
        public IEnumerable<CustomerDTO> GetCustomer()
        {
            return Mapper.Map<IEnumerable<CustomerDTO>>(_context.Customer.OrderBy(x => x.LastName).ThenBy(x => x.FirstName));
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }

            var customer = await _context.Customer.SingleOrDefaultAsync(m => m.Id == id);

            if (customer == null)
            {
                return BadRequest(ErrorHelper.Response(404,"Registro no encontrado"));
            }

            return Ok(Mapper.Map<CustomerDTO>(customer));
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] dynamic credentials)
        {
            var username = (string)credentials["username"];
            var password = (string)credentials["password"];

            var customer = await _context.Customer.SingleOrDefaultAsync(m => m.Username == username && m.Password == password);

            if (customer == null)
            {
                return BadRequest(ErrorHelper.Response(404,"Registro no encontrado"));
            }

            return Ok(Mapper.Map<CustomerDTO>(customer));
        }

        // PUT: api/Customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer([FromRoute] int id, [FromBody] CustomerDTO customer)
        {
            customer.CustomerOrder = null;

            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }

            if (id != customer.Id)
            {
                return BadRequest();
            }

            _context.Entry(Mapper.Map<Customer>(customer)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return BadRequest(ErrorHelper.Response(404,"Registro no encontrado"));
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Customers
        [HttpPost]
        public async Task<IActionResult> PostCustomer([FromBody] CustomerDTO customer)
        {
            customer.CustomerOrder = null;

            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }

            var c = Mapper.Map<Customer>(customer);
            _context.Customer.Add(c);
            await _context.SaveChangesAsync();
            customer.Id = c.Id;

            return CreatedAtAction(nameof(GetCustomer), new { id = c.Id }, customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }

            var customer = await _context.Customer.SingleOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return BadRequest(ErrorHelper.Response(404,"Registro no encontrado"));
            }

            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();

            return Ok(Mapper.Map<CustomerDTO>(customer));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customer.Any(e => e.Id == id);
        }
    }
}
