using Cundi_incidencias.Dto;
using Cundi_incidencias.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cundi_incidencias.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
        public class EmpleadoController : ControllerBase
        {
        private readonly EmpleadoService _empleadoService;
        public EmpleadoController(EmpleadoService empleadoService)
        {
            _empleadoService = empleadoService;
        }

        [HttpPost("CrearEmpleado")]
        
        public async Task<IActionResult> CrearEmpleado([FromBody] EmpleadoDto empleado)
        {
            try
            {
                await _empleadoService.CrearEmpleado(empleado);
                return Ok(new { mensaje = "EMPLEADO REGISTRADO EXITOSAMENTE" });

            }
            catch (Exception ex)
            {
                return StatusCode(500, "ERROR: " + ex.Message);
            }
        }
        [HttpPost("Actualizar")]
        
        public async Task<IActionResult> ActualizarEmpleado([FromForm] int id_usuario, [FromForm] string direccion, [FromForm] string telefono)
        {
            try
            {
                await _empleadoService.ActualizarEmpleado(id_usuario, direccion, telefono);
                return Ok(new { mensaje = "EMPLEADO ACTUALIZADO EXITOSAMENTE" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "ERROR: " + ex.Message);
            }
        }
        [HttpDelete("Eliminar")]
        
        public async Task<IActionResult> EliminarUsuario([FromForm] int id_usuario)
        {
            try
            {
                await _empleadoService.EliminarEmpleado(id_usuario);
                return Ok(new { mensaje = "USUARIO ELIMINADO EXITOSAMENTE" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "ERROR: " + ex.Message);
            }
        }
        [HttpGet("ListaEmpleados")]
        
        public async Task<IActionResult> MostrarEmpleados()
        {
            List<EmpleadoDto> listEmpleados = new List<EmpleadoDto>();
            listEmpleados = await _empleadoService.MostrarEmpleado();
            if (listEmpleados.Count == 0)
            {
                return NotFound("NO HAY EMPLEADOS REGISTRADOS");
            }
            return Ok(listEmpleados);
        }

        [HttpPost("Asignar")]
        
        public async Task<IActionResult> Asignar([FromForm] int id_usuario, [FromForm] int id_incidencia)
        {
            try
            {

                if (await _empleadoService.Asignar(id_usuario, id_incidencia) !=0)
                {
                    return Ok(new { mensaje = "ASIGNADO EXITOSAMENTE" });
                }
                else
                {
                    return BadRequest(new { message = "FALLO" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "ERROR: " + ex.Message);
            }
        }
        [HttpGet("ObtenerIncidenciasAsignadas")]
        
        public async Task<IActionResult> MostrarIncidencias(int id_usuario)
        {
            List<IncidenciaDto> listIncidencias = new List<IncidenciaDto>();
            listIncidencias = await _empleadoService.ObtenerIncidencia(id_usuario);
            if (listIncidencias.Count == 0)
            {
                return NotFound("NO HAY INCIDENCIAS REGISTRADAS");
            }
            return Ok(listIncidencias);
        }
        [HttpPost("ActualizarInci")]
        
        public async Task<IActionResult> ActualizarInci([FromForm] int id_incidencia, [FromForm] string descripcion, [FromForm] string imagen)
        {
            try
            {
                await _empleadoService.ActualizarIncidencia(id_incidencia, descripcion, imagen);
                return Ok(new { mensaje = "INCIDENCIA ACTUALIZADA EXITOSAMENTE" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "ERROR: " + ex.Message);
            }
        }
    }
}
