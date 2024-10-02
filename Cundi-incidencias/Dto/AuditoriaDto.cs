namespace Cundi_incidencias.Dto
{
    public class AuditoriaDto
    {
        public int id_auditoria { get; set; }
        public int id_usuario { get; set; }
        public string tabla { get; set; }
        public string accion { get; set; }
        public string descripcion { get; set;}
        public string fecha { get; set; }
    }
}
