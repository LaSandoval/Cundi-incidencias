using Cundi_incidencias.Dto;
using Cundi_incidencias.Repository;
using Cundi_incidencias.Utility;
using Microsoft.AspNetCore.Mvc;
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
            incidencia1 = await _incidenciaRepository.CrearIncidencia(incidencia);
            return incidencia;

        }

        public async Task<List<IncidenciaDto>> ObtenerHistorialIncidencia()
        {
            var incidencia = await _incidenciaRepository.ObtenerHistorialIncidencia();
            return incidencia;

        }
        public async Task<int> ActualizarIncidencia([FromBody] IncidenciaDto incidencia)
        {
           int filasactualizadas = await _incidenciaRepository.ActualizarIncidencia(incidencia);
            return filasactualizadas;
        }
    }
}
