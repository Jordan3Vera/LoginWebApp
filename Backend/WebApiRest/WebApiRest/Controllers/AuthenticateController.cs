using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiRest.Data;
using WebApiRest.Helper;
using WebApiRest.Model;
using WebApiRest.Model.ViewModel;

namespace WebApiRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthenticateController : ControllerBase
    {
        private readonly WebApiRestContext _context;
        private readonly IConfiguration _config;

        public AuthenticateController(WebApiRestContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpGet]
        public IEnumerable<object> GetUsuarios()
        {
            try
            {
                var usuarios = new List<object>()
                {
                    new
                    {
                        Username = "JVC32",
                        Email = "jv@hotmail.com",
                        Password = "1234",
                        ConfirmPassword = "1234"
                    },
                    new 
                    {
                        Username = "Pedre22",
                        Email = "pr@hotmail.com",
                        Password = "1234",
                        ConfirmPassword = "1234"
                    },
                    new 
                    {
                        Username = "Carancho12",
                        Email = "ch@hotmail.com",
                        Password = "1234",
                        ConfirmPassword = "1234"
                    }
                };

                return usuarios;

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Login(LoginVM login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
            }

            Usuario user = await _context.Usuario.Where(x => x.Email == login.Email).FirstOrDefaultAsync();

            if(user == null)
            {
                Usuario pass = await _context.Usuario.Where(x => x.Password != login.Password).FirstOrDefaultAsync();
                if (pass == null)
                {
                    return BadRequest(ErrorHelper.Response(404, "La contraseña es incorrecta"));
                }
                return BadRequest(ErrorHelper.Response(404, "Registro no encontrado"));
            }

            var secret = _config.GetValue<string>("SecretKey");
            var jwtHelper = new JWTHelper(secret);
            var token = jwtHelper.CreateToken(login.Email);
            List<Usuario> usuario = new List<Usuario>();

            return Ok(new
            {
                ok = true,
                msg = "Logueado con exito",
                token = token,
                email = login.Email
            });
        }
    }
}
