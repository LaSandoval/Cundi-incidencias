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

        [HttpPut("ActualizarInci")]
        public async Task<IActionResult> ActualizarInci([FromForm] int id_incidencia,[FromForm] string nombre_incidencia, [FromForm] string descripcion, [FromForm] string imagen, [FromForm] int id_categoria, [FromForm] int id_ubicacion)
        {
            try
            {
                if(await _incidenciaService.ActualizarIncidencia(id_incidencia, nombre_incidencia, descripcion, imagen, id_categoria, id_ubicacion) > 0)
                {
                    return Ok(new { mensaje = "INCIDENCIA ACTUALIZADA EXITOSAMENTE" });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "ERROR: " + ex.Message);
            }
        }

        [HttpGet("ObtenerIncidencia/{id_incidencia}")]
        public async Task<IActionResult> ObtenerIncidencia(int id_incidencia)
        {
            try
            {
                var incidencia = await _incidenciaService.ObtenerIncidenciaId(id_incidencia);
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

        [HttpDelete("EliminarIncidencia")]
        public async Task<IActionResult> EliminarIncidencia([FromQuery] int id_incidencia)
        {
            try
            {
                var result = await _incidenciaService.EliminarIncidencia(id_incidencia);
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
   