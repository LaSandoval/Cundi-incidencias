using Cundi_incidencias.Dto;
using Cundi_incidencias.Repository;
using Cundi_incidencias.Utility;
using System.Text;


namespace Cundi_incidencias.Services
{
    public class IncidenciaService
    {
        private readonly IncidenciaRepository _incidenciaRepository;

        public IncidenciaService(IncidenciaRepository incidenciaRepository)
        {
            _incidenciaRepository = incidenciaRepository;
        }
        public async Task<IncidenciaDto> CrearIncidencia(IncidenciaDto incidencia)
        {
            IncidenciaDto incidencia1 = new IncidenciaDto();
            incidencia.id_estado = 1;
            incidencia.fecha_inicio=DateTime.Now;
            incidencia1 = await _incidenciaRepository.CrearIncidencia(incidencia);
            return incidencia;

        }
        public async Task<int> ActualizarIncidencia(int id_incidencia, string nombre_incidencia,string descripcion, string imagen, int id_categoria, int id_ubicacion)
        {
            int filasactualizadas;
            filasactualizadas = await _incidenciaRepository.ActualizarIncidencia(id_incidencia, nombre_incidencia, descripcion, imagen, id_categoria, id_ubicacion);
            return filasactualizadas;
        }

        public async Task<IncidenciaDto> ObtenerIncidenciaId(int id_incidencia)
        {
            return await _incidenciaRepository.ObtenerIncidenciaId(id_incidencia);
        }
        public async Task<List<IncidenciaDto>> ObtenerHistorialIncidencia()
        {
            var incidencia = await _incidenciaRepository.ObtenerHistorialIncidencia();
            return incidencia;
        }

        public async Task<int> EliminarIncidencia(int id_incidencia)
        {
           int filasEliminadas;
           filasEliminadas= await _incidenciaRepository.EliminarIncidencia(id_incidencia);
            return filasEliminadas;
        }
    }
}
