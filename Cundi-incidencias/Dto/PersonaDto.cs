namespace Cundi_incidencias.Dto
{
    public class PersonaDto
    {
        public int id_usuario { get; set; }
        public int id_rol { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string correo { get; set; }
        public string contrasena { get; set; }
        public string telefono { get; set; }
        public string direccion { get; set; }
        public int? id_estado { get; set; }
        public int? token { get; set; }
        public DateTime fecha_registro
        {
            get; set;
        }
    }
}
