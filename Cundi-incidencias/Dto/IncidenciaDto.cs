namespace Cundi_incidencias.Dto
{
    public class IncidenciaDto
    {
        public int id_incidencia { get; set; }
        public string nombre_incidencia { get; set; }
        public string descripcion { get; set; }
        public string imagen { get; set;}
        public string fecha_inicio { get; set; }
        public string fecha_fin{ get; set; }
        public int id_usuario { get; set; }
        public int id_estado { get; set; }
        public int id_categoria { get; set; }
        public int id_ubicacion { get; set; }
    }
}
