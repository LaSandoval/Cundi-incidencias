using Cundi_incidencias.Dto;
using Cundi_incidencias.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cundi_incidencias.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class IncidenciaController : ControllerBase
    {
        private readonly IncidenciaService _incidenciaService;
        public IncidenciaController(IncidenciaService incidenciaService)
        {
            _incidenciaService = incidenciaService;
        }

        [HttpPost("RegistrarIncidencia")]
        public async Task<IActionResult> RegistrarIncidencia([FromBody] IncidenciaDto incidencia)
        {
            try
            {
                await _incidenciaService.CrearIncidencia(incidencia);
                return Ok(new { mensaje = "INCIDENCIA REGISTRADA EXITOSAMENTE" });

            }
            catch (Exception ex)
            {
                return StatusCode(500, "ERROR: " + ex.Message);
            }
        }
        [HttpPost("ActualizarInci")]
        public async Task<IActionResult> ActualizarInci([FromForm] string nombre_incidencia, [FromForm] string imagen, [FromForm] int id_categoria, [FromForm] int id_ubicacion)
        {
            try
            {
                await _incidenciaService.ActualizarIncidencia(nombre_incidencia, imagen, id_categoria, id_ubicacion);
                return Ok(new { mensaje = "INCIDENCIA ACTUALIZADA EXITOSAMENTE" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "ERROR: " + ex.Message);
            }
        }
    }
}
   