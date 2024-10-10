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

        [HttpGet("ObtenerIncidencia/{nombre_incidencia}")]
        public async Task<IActionResult> ObtenerIncidencia(string nombre_incidencia)
        {
            try
            {
                var incidencia = await _incidenciaService.ObtenerIncidenciaPorNombre(nombre_incidencia);
                if (incidencia != null)
                {
                    return Ok(incidencia);
                }
                else
                {
                    return NotFound(new { mensaje = "INCIDENCIA NO ENCONTRADA" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "ERROR: " + ex.Message);
            }
        }

        [HttpDelete("EliminarIncidencia/{nombre_incidencia}")]
        public async Task<IActionResult> EliminarIncidencia(string nombre_incidencia)
        {
            try
            {
                var result = await _incidenciaService.EliminarIncidencia(nombre_incidencia);
                if (result > 0)
                {
                    return Ok(new { mensaje = "INCIDENCIA ELIMINADA EXITOSAMENTE" });
                }
                else
                {
                    return NotFound(new { mensaje = "INCIDENCIA NO ENCONTRADA" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "ERROR: " + ex.Message);
            }
        }
    }
}
   