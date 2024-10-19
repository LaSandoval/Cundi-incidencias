using Cundi_incidencias.Dto;
using Cundi_incidencias.Services;
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
        public async Task<IActionResult> MostrarEjercicios()
        {
            List<EmpleadoDto> listEmpleados = new List<EmpleadoDto>();
            listEmpleados = await _empleadoService.MostrarEmpleado();
            if (listEmpleados.Count == 0)
            {
                return NotFound("No hay empleados registrados");
            }
            return Ok(listEmpleados);
        }
    }
}
