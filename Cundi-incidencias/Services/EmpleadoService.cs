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
    }
}
