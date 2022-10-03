using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiRest.Data;
using WebApiRest.Helper;
using WebApiRest.Model;
using WebApiRest.Model.DTOs;
using WebApiRest.Model.ViewModel;

namespace WebApiRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsuariosController : Controller
    {
        private readonly WebApiRestContext _context;
        private readonly IConfiguration _config;
        private readonly ILogger _logger;

        public UsuariosController(WebApiRestContext context, IConfiguration config, ILoggerFactory logFactory)
        {
            _context = context;
            _config = config;
            _logger = logFactory.CreateLogger<UsuariosController>();
        }

        [HttpGet]
        [AllowAnonymous]
        public IEnumerable<UsuarioDTO> GetUser()
        {
            var usuario = Mapper.Map<IEnumerable<UsuarioDTO>>(
                _context.Usuario.Select(x => new UsuarioDTO
                {
                    UserId = x.UserId,
                    UserName = x.Username,
                    Email = x.Email,
                    Password = x.Password
                }).ToList());
            _logger.LogInformation("Esta es la lista de todos los usuarios");
            return usuario;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUserId([FromRoute] int id)
        {
            try
            {
                var usuario = await _context.Usuario.SingleOrDefaultAsync(x => x.UserId == id);

                if(usuario == null)
                {
                    _logger.LogWarning($"El usuario de ID {id} no a sido encontrado");
                    return BadRequest(ErrorHelper.Response(404, $"El registro {id} no encontrado"));
                }
                else
                {
                    return Ok(Mapper.Map<UsuarioVM>(usuario));
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<Usuario>> PostUser([FromBody] Usuario oUser)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
                }

                if (await _context.Usuario.Where(x => x.Email == oUser.Email).AnyAsync())
                {
                    return BadRequest(ErrorHelper.Response(400, $"Este correo ya se encuentra en uso"));
                }else if(await _context.Usuario.Where(x => x.Username == oUser.Username).AnyAsync())
                {
                    return BadRequest(ErrorHelper.Response(400, $"{oUser.Username}, ya se encuentra en uso"));
                };

                HashedPassword pass = HashHelper.Hash(oUser.Password);
                oUser.Password = pass.Password;
                oUser.ConfirmPassword = pass.Password;

                _context.Usuario.Add(oUser);
                _logger.LogInformation("Este método crea un usuario");
                await _context.SaveChangesAsync();

                //Esto es lo que muestra cuando un registro es creado
                return CreatedAtAction(nameof(PostUser), new UsuarioVM()
                {
                    UserId = oUser.UserId,
                    UserName = oUser.Username,
                    Email = oUser.Email,
                    Password = oUser.Password
                });
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> PutUser([FromRoute] int id, [FromBody] Usuario oUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }

            if(id != oUser.UserId)
            {
                return BadRequest(ErrorHelper.Response(404,"Este registro no existe"));
            }

            //_context.Entry(oUser).State = EntityState.Modified;
            _context.Entry(oUser).Property(x => x.Password).IsModified = true;

            try
            {
                HashedPassword pass = HashHelper.Hash(oUser.Password);
                oUser.Password = pass.Password;
                oUser.ConfirmPassword = pass.Password;


                await _context.SaveChangesAsync();
                return Ok(ErrorHelper.Response(200, "Registro actualizado correctamente"));
            }
            catch(DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound(ErrorHelper.Response(404, "Usuario no encontrado"));
                }
                else
                {
                    throw;
                }
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Usuario>> DeleteUser([FromRoute] int id)
        {
            var user = await _context.Usuario.FindAsync(id);
            if(user == null)
            {
                _logger.LogWarning($"El usuario de ID {id} no sido encontrado");
                return BadRequest(ErrorHelper.Response(404, "Registro no encontrado"));
            }

            _context.Usuario.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private bool UserExists(int id)
        {
            return _context.Usuario.Any(u => u.UserId == id);
        }
    }
}
