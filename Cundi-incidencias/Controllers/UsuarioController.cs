using Cundi_incidencias.Dto;
using Cundi_incidencias.Services;
using Microsoft.AspNetCore.Authorization;
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
        [HttpGet("ActualizarCuenta")]
        
        public async Task<IActionResult> ActualizarCuenta([FromQuery] int token)
        {
            try
            {
                if (await _usuarioService.ActivarCuenta(token) != 0)
                {
                    return Redirect("http://localhost:5173/");
                }
                else
                {
                    return BadRequest("ERROR");
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
                   var token =  await _usuarioService.IniciarSesion(correo, contrasena); 
                return Ok(new { token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "ERROR: " + ex.Message);
            }
        }

        [HttpPost("Actualizar")]
        [ValidarCorreo]
        
        public async Task<IActionResult> ActualizarUsuario([FromForm] string correo, [FromForm] int programa,
        [FromForm] int semestre, [FromForm] string direccion, [FromForm] string telefono)
        {
            try
            {
                if (await _usuarioService.ActualizarUsuario(correo, programa, semestre, direccion, telefono) == 1)
                {
                    return Ok(new { mensaje = "USUARIO ACTUALIZADO EXITOSAMENTE" });
                }
                else
                {
                    return BadRequest(new { message = "ERROR" }); ;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "ERROR: " + ex.Message);
            }
        }

        [HttpDelete("Eliminar")]
        [ValidarCorreo]
        
        public async Task<IActionResult> EliminarUsuario([FromQuery] int id_usuario)
        {
            try
            {
                string descripcion = $"Eliminar al usuario {id_usuario}";
                await _usuarioService.EliminarUsuario(id_usuario, descripcion);
                return Ok(new { mensaje = "USUARIO ELIMINADO EXITOSAMENTE" });
                    }

            catch (Exception ex)
            {
                return StatusCode(500, "ERROR: " + ex.Message);
            }
        }
    

        [HttpGet("Traer-Datos-Usuario")]
        
        public async Task<IActionResult> ObtenerDatosPersona([FromQuery] int id_usuario)
        {
            var usuario = await _usuarioService.EnviarDatosUsu(id_usuario);

            if (usuario == null)
            {
                return NotFound("USUARIO NO ENCONTRADO");
            }

            return Ok(new
            {
                id_rol = usuario.persona.id_rol,
                nombres = usuario.persona.nombre,
                apellidos = usuario.persona.apellido,
                correo = usuario.persona.correo,
                direccion = usuario.persona.direccion,
                fecha_registro = usuario.persona.fecha_registro,
                telefono = usuario.persona.telefono,
                id_estado = usuario.persona.id_estado,
                token = usuario.persona.token,
                id_programa = usuario.id_programa,
                id_semestre = usuario.id_semestre
            });
        }
        [HttpGet("ListaIncidenciaUsuario")]
        
        public async Task<IActionResult> MostrarIncidencias(int id_usuario)
        {
            List<IncidenciaDto> listIncidencias = new List<IncidenciaDto>();
            listIncidencias = await _usuarioService.MostrarIncidencia(id_usuario);
            if (listIncidencias.Count == 0)
            {
                return NotFound("NO HAY INCIDENCIAS REGISTRADAS");
            }
            return Ok(listIncidencias);
        }

    }
}
