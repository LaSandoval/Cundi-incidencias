namespace Cundi_incidencias.Dto
{
    public class RecuperarContrasenaDto
    {
        public int id_recuperacion {  get; set; }
        public int id_usuario { get; set; }
        public string token { get; set; }
        public string fecha_solicitud { get; set; }
        public string fecha_expiracion { get; set; }
    }
}
