using Cundi_incidencias.Dto;
using Cundi_incidencias.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cundi_incidencias.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;
        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost("Registrar")]
        public async Task<IActionResult> RegistrarUsuario([FromBody] UsuarioInDto usuario)
        {
            try
            {
                if (await _usuarioService.BuscarPersona(usuario.persona.correo) == false)
                {
                    await _usuarioService.RegistroUsuario(usuario);
                    return Ok(new { mensaje = "USUARIO REGISTRADO EXITOSAMENTE" });
                }
                else
                {
                    return BadRequest("CORREO REGISTRADO");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "ERROR: " + ex.Message);
            }
        }

       [HttpPost("Login")]
       public async Task<IActionResult> login([FromForm] string correo, [FromForm] string contrasena)
           {
            try
            {
                if (await _usuarioService.IniciarSesion(correo, contrasena) == true)
                {
                    return Ok(new { message = "INICIO DE SESIÓN EXITOSO" });
                }
                else
                {
                    return BadRequest(new { message = "INICIO DE SESIÓN FALLIDO" });
                }
            }
            catch(Exception ex)
            {
                return StatusCode(500, "ERROR: " + ex.Message);
            }
           }
       
    [HttpPost("Actualizar")]
        public async Task<IActionResult> ActualizarUsuario([FromForm] string correo, [FromForm] string programa, [FromForm] string semestre, [FromForm] string direccion, [FromForm] string telefono)
        {
            try
            {
                await _usuarioService.ActualizarUsuario(correo, programa, semestre, direccion, telefono);
                return Ok(new { mensaje = "USUARIO ACTUALIZADO EXITOSAMENTE" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "ERROR: " + ex.Message);
            }
        }
        [HttpDelete("Eliminar")]
        public async Task<IActionResult> EliminarUsuario([FromForm] string correo)
        {
            try
            {
                await _usuarioService.EliminarUsuario(correo);
                return Ok(new { mensaje = "USUARIO ELIMINADO EXITOSAMENTE" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "ERROR: " + ex.Message);
            }
        }
    }
}
