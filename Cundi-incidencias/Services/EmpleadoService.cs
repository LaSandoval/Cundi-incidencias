using Cundi_incidecnias.Utility;
using Cundi_incidencias.Dto;
using Cundi_incidencias.Repository;
using Cundi_incidencias.Utility;

namespace Cundi_incidencias.Services
{
    public class EmpleadoService
    {
        private readonly EmpleadoRepository _empleadoRepository;

        public EmpleadoService(EmpleadoRepository empleadoRepository)
        {
            _empleadoRepository = empleadoRepository;
        }
        public async Task<EmpleadoDto> CrearEmpleado(EmpleadoDto empleado)
        {
            EmpleadoDto empleado1 = new EmpleadoDto();
            BycriptUtility bycriptUtility = new BycriptUtility();
            empleado.persona.contrasena = bycriptUtility.HashPassword(empleado.persona.contrasena);
            empleado.persona.id_rol = 2;
            empleado.persona.id_estado = 1;
            empleado1 = await _empleadoRepository.CrearEmpleado(empleado);
            return empleado;

        }
        public async Task<int> ActualizarEmpleado(int id_usuario, string direccion, string telefono)
        {
            int filasactualizadas = 0;
            filasactualizadas = await _empleadoRepository.ActualizarEmpleado(id_usuario, direccion, telefono);
            return filasactualizadas;

        }
        public async Task<int> EliminarEmpleado(int id_usuario)
        {
            int filaseliminadas = 0;
            filaseliminadas = await _empleadoRepository.EliminarEmpleado(id_usuario);
            return filaseliminadas;

        }
        public async Task<List<EmpleadoDto>> MostrarEmpleado()
        {
            List<EmpleadoDto> listEmpleados = new List<EmpleadoDto>();
            listEmpleados = await _empleadoRepository.MostrarEmpleado();
            return listEmpleados;
        }
        public async Task<int> Asignar(int id_usuario, int id_incidencia)
        {
            int comando = 0;
            comando = await _empleadoRepository.Asignar(id_usuario, id_incidencia);
            return comando;

        }
        public async Task<List<IncidenciaDto>> ObtenerIncidencia(int id_usuario)
        { 
            List<IncidenciaDto> listIncidencias = new List<IncidenciaDto>();
            listIncidencias = await _empleadoRepository.TraerIncidenciasAsignadas(id_usuario);
            return listIncidencias;

        }
        public async Task<int> ActualizarIncidencia(int id_incidencia, string descripcion, string imagen)
        {
            DateTime fecha_fin = DateTime.Now;
            int resultado = 0;
            resultado = await _empleadoRepository.ActualizarIncidencia(id_incidencia, descripcion, imagen, fecha_fin);
            return resultado;
        }
    }
}
