﻿using Cundi_incidencias.Dto;
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
            incidencia1 = await _incidenciaRepository.CrearIncidencia(incidencia);
            return incidencia;

        }
        public async Task<int> ActualizarIncidencia(string nombre_incidencia, string imagen, int id_categoria, int id_ubicacion)
        {
            int filasactualizadas = 0;
            filasactualizadas = await _incidenciaRepository.ActualizarIncidencia(nombre_incidencia, imagen, id_categoria, id_ubicacion);
            return filasactualizadas;
        }
    }
}
