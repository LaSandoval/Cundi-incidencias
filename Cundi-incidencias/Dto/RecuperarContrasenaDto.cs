namespace Cundi_incidencias.Dto
{
    public class RecuperarContrasenaDto
    {
        public int id_recuperacion {  get; set; }
        public int id_usuario { get; set; }
        public int token { get; set; }
        public DateTime fecha_solicitud { get; set; }
        public DateTime fecha_expiracion { get; set; }
    }
}
