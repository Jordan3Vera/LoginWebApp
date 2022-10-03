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
    [Consumes("application/json")]
    [Route("api/Products")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly WebApiRestContext _context;

        public ProductsController(WebApiRestContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public IEnumerable<ProductDTO> GetProduct()
        {
            return Mapper.Map<IEnumerable<ProductDTO>>(_context.Product.OrderBy(x => x.Name));
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }

            var product = await _context.Product.SingleOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return BadRequest(ErrorHelper.Response(404,"Registor no encontrado"));
            }

            return Ok(Mapper.Map<ProductDTO>(product));
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct([FromRoute] int id, [FromBody] ProductDTO product)
        {
            product.OrderDetail = null;

            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }

            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(Mapper.Map<Product>(product)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return BadRequest(ErrorHelper.Response(404,"Registro no encontrado"));
                }
                else
                {
                    throw;
                }
            }

            return BadRequest(ErrorHelper.Response(400,"No se pudo actualizar el registro"));
        }

        // POST: api/Products
        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody] ProductDTO product)
        {
            product.OrderDetail = null;

            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }

            var p = Mapper.Map<Product>(product);

            _context.Product.Add(p);
            await _context.SaveChangesAsync();
            product.Id = p.Id;

            return CreatedAtAction(nameof(GetProduct), new { id = p.Id }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }

            var product = await _context.Product.SingleOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(Mapper.Map<ProductDTO>(product));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
