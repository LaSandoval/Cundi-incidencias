using Cundi_incidencias.Services;
using Cundi_incidencias.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Cundi_incidencias.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonaController : ControllerBase
    {
        private readonly PersonaService _personaService;
        public PersonaController(PersonaService personaService)
        {
            _personaService = personaService;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> login([FromForm] string correo, [FromForm] string contrasena)
        {
            try
            {
                if (await _personaService.IniciarSesion(correo, contrasena) == true)
                {
                    return Ok(new { message = "INICIO DE SESIÓN EXITOSO" });
                }
                else
                {
                    return BadRequest(new { message = "INICIO DE SESIÓN FALLIDO" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "ERROR: " + ex.Message);
            }
        }
        [HttpGet("TraerDatosPersona")]
        public async Task<IActionResult> TraerDatos([FromQuery] string correo)
        {
            try
            {
                var persona = await _personaService.TraerDatosPersona(correo);

                if (persona == null)
                {
                    return NotFound(new { message = "PERSONA NO ENCONTRADA" });
                }
                return Ok(new
                {
                    id_usuario1 = persona.id_usuario,
                    id_rol1 = persona.id_rol,
                    message = "DATOS ENCONTRADOS"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "ERROR: " + ex.Message);
            }
        }

    }
}
