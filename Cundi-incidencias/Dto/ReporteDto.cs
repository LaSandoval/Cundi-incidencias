namespace Cundi_incidencias.Dto
{
    public class ReporteDto
    {
        public int id_reporte { get; set; }
        public int id_incidencia { get; set; }
        public int id_usuario { get; set; }
        public string descripcion { get; set; }
        public string fecha_reporte { get; set; }
        public int id_estado_reporte { get; set; }
        public int id_categoria { get; set; }

    }
}
