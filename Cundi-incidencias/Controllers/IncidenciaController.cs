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

        [HttpGet("ObtenerHistorialIncidencia")]
        public async Task<IActionResult> ObtenerHistorialIncidencia()
        {
            try
            {
                var incidencia = await _incidenciaService.ObtenerHistorialIncidencia();

                if (incidencia == null)
                {
                    return NotFound(new { message = "NO SE HAN ENCONTRADO REGISTROS" });
                }
                return Ok(new
                {
                    incidencia
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "ERROR: " + ex.Message);
            }
        }

        [HttpPost("ActualizarInci")]
        public async Task<IActionResult> ActualizarInci([FromBody] IncidenciaDto incidencia)
        {
            try
            {
               var incidenciaRespuesta = await _incidenciaService.ActualizarIncidencia(incidencia);
                if (incidenciaRespuesta.Equals(1))
                {
                    return Ok(new { mensaje = "INCIDENCIA ACTUALIZADA EXITOSAMENTE" });
                }
                else
                {
                    return StatusCode(401);
                }
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, "ERROR: " + ex.Message);
            }
        }
    }
}
   