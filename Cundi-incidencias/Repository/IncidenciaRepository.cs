using Cundi_incidencias.Dto;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using Cundi_incidencias.Utility;
using Cundi_incidencias.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using static System.Net.Mime.MediaTypeNames;
using Cundi_incidencias.Services;


namespace Cundi_incidencias.Repository
{
    public class IncidenciaRepository
    {
        private readonly string _connectionString;
        private readonly IEmailService _iEmailService;
        public IncidenciaRepository(string connectionString, IEmailService emailService)
        {
            _connectionString = connectionString;
            _iEmailService = emailService;
        }

        public async Task<IncidenciaDto> CrearIncidencia(IncidenciaDto incidencia)
        {
            string insertQuery = @"INSERT INTO incidencia (nombre_incidencia, descripcion, imagen, fecha_inicio, fecha_fin, id_usuario, id_estado, id_categoria, id_ubicacion, fecha_creacion)     
                                    VALUES (@nombre_incidencia, @descripcion, @imagen, @fecha_inicio, @fecha_fin, @id_usuario, @id_estado, @id_categoria, @id_ubicacion, @fecha_creacion);
                                    SELECT SCOPE_IDENTITY();";

            var idIncidencia = string.Empty;
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                var fecha = DateTime.Now;
                await con.OpenAsync();
                using (SqlCommand programaCmd = new SqlCommand(insertQuery, con))
                {
                    using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@nombre_incidencia", incidencia.nombre_incidencia);
                        cmd.Parameters.AddWithValue("@descripcion", incidencia.descripcion);
                        cmd.Parameters.AddWithValue("@imagen", ConvertFileToBase64(@"C:\pexels-photo-1906795.jpeg").ToString());//incidencia.imagen
                        cmd.Parameters.AddWithValue("@fecha_inicio", incidencia.fecha_inicio);
                        cmd.Parameters.AddWithValue("@fecha_fin", incidencia.fecha_fin);
                        cmd.Parameters.AddWithValue("@id_usuario", incidencia.id_usuario);
                        cmd.Parameters.AddWithValue("@id_estado", incidencia.id_estado);
                        cmd.Parameters.AddWithValue("@id_categoria", incidencia.id_categoria);
                        cmd.Parameters.AddWithValue("@id_ubicacion", incidencia.id_ubicacion);
                        cmd.Parameters.AddWithValue("@fecha_creacion", fecha);

                        object idGenerado = await cmd.ExecuteScalarAsync();
                        idIncidencia = idGenerado.ToString();
                    }

                    var emailDto = new EmailDto
                    {
                        Para = "vanessara2812@gmail.com", //aqui se le debe dar manejo al correo, por ejemplo
                                                          //el correo del usuario que está en la sesion o similar
                        Asunto = "Creacion de incidencia",
                        Contenido = "<html>" +
                                        "<head>" +
                                        "<style>" +
                                        "body {" +
                                        "margin: 0;" +
                                        "padding: 0;" +
                                        "font-family: Arial, sans-serif;" +
                                        "background-color: #ffffff;" +
                                        "}" +
                                        ".container {" +
                                        "max-width: 30rem;" +
                                        "margin: 0 auto;" +
                                        "padding: 2rem;" +
                                        "background-color: #ffffff;" +
                                        "border-radius: 1rem;" +
                                        "box-shadow: 0 0 2rem rgba(0, 128, 0, 0.2);" +
                                        "color: #000000;" +
                                        "}" +
                                        ".header {" +
                                        "text-align: center;" +
                                        "background-color: #008000;" +
                                        "padding: 1rem;" +
                                        "border-top-left-radius: 1rem;" +
                                        "border-top-right-radius: 1rem;" +
                                        "color: #ffffff;" +
                                        "}" +
                                        ".logo {" +
                                        "width: 80px;" +
                                        "vertical-align: middle;" +
                                        "margin-right: 0.5rem;" +
                                        "}" +
                                        ".header h1 {" +
                                        "display: inline-block;" +
                                        "vertical-align: middle;" +
                                        "margin: 0;" +
                                        "font-size: 1.5rem;" +
                                        "}" +
                                        ".content {" +
                                        "padding: 1rem;" +
                                        "background-color: #ffffff;" +
                                        "border-radius: 1rem;" +
                                        "box-shadow: 0 2px 5px rgba(0, 128, 0, 0.1);" +
                                        "border: 1px solid #008000;" +
                                        "}" +
                                        ".content h2 {" +
                                        "color: #008000;" +
                                        "margin-bottom: 1.3rem;" +
                                        "}" +
                                        ".content p {" +
                                        "color: #000000;" +
                                        "font-size: 1rem;" +
                                        "line-height: 1.6;" +
                                        "}" +
                                        ".footer {" +
                                        "text-align: center;" +
                                        "margin-top: 2rem;" +
                                        "padding-top: 1rem;" +
                                        "border-top: 1px solid #008000;" +
                                        "}" +
                                        ".footer p {" +
                                        "color: #008000;" +
                                        "font-size: 0.8rem;" +
                                        "}" +
                                        ".codigo {" +
                                        "text-align: center;" +
                                        "font-weight: bold;" +
                                        "color: #008000;" +
                                        "font-size: 1.2rem;" +
                                        "}" +
                                        "</style>" +
                                        "</head>" +
                                        "<body>" +
                                        "<div class='container'>" +
                                        "<div class='header'>" +
                                        "<img src='https://i.ibb.co/f8ytCBW/Cundi.png' alt='Cundi-Incidencias' class='logo'/>" +
                                        "<h1>Cundi-Incidencias</h1>" +
                                        "</div>" +
                                        "<div class='content'>" +
                                        "<h2>¡Hola!</h2>" +
                                        "<p>Se ha registrado una nueva incidencia</p>" +
                                        "<p>A continuación, encontrarás el id</p>" +
                                        "<p class='codigo'>" + idIncidencia + "</p>" +
                                        "<p>Gracias por utilizar Cundi-Incidencias.</p>" +
                                        "</div>" +
                                        "<div class='footer'>" +
                                        "<p>Este correo electrónico fue enviado por Cundi-Incidencias. Para cualquier consulta, escríbenos a cundiincidenciassoporte@gmail.com.</p>" +
                                        "</div>" +
                                        "</div>" +
                                        "</body>" +
                                        "</html>"
                    };
                    _iEmailService.EnviarEmail(emailDto);
                }

                return incidencia;
            }
        }
        public async Task<int> ActualizarIncidencia(IncidenciaDto incidencia)
        {
            int filasactualizadas = 0;

            string updateQuery = @"UPDATE incidencia SET 
                                   NOMBRE_INCIDENCIA = @nombre_incidencia, 
                                   DESCRIPCION = @descripcion,
                                   IMAGEN = @imagen,
                                   FECHA_INICIO = @fecha_inicio,
                                   FECHA_FIN = @fecha_fin, 
                                   ID_ESTADO = @id_estado,
                                   ID_CATEGORIA = @id_categoria,
                                   ID_UBICACION = @id_ubicacion 
                                   WHERE 
                                   ID_INCIDENCIA = @id_incidencia 
                                   AND DATEDIFF(MINUTE, FECHA_CREACION, GETDATE()) < 10";
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(updateQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@id_incidencia", incidencia.id_incidencia);
                        cmd.Parameters.AddWithValue("@nombre_incidencia", incidencia.nombre_incidencia);
                        cmd.Parameters.AddWithValue("@descripcion", incidencia.descripcion);
                        cmd.Parameters.AddWithValue("@imagen", ConvertFileToBase64(@"C:\pexels-photo-1906795.jpeg")); //foto de prueba
                        cmd.Parameters.AddWithValue("@fecha_inicio", incidencia.fecha_inicio);
                        cmd.Parameters.AddWithValue("@fecha_fin", incidencia.fecha_fin);
                        cmd.Parameters.AddWithValue("@id_estado", incidencia.id_estado);
                        cmd.Parameters.AddWithValue("@id_categoria", incidencia.id_categoria);
                        cmd.Parameters.AddWithValue("@id_ubicacion", incidencia.id_ubicacion);
                        filasactualizadas = await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar la incidencia: " + ex.Message);
            }

            return filasactualizadas;
        }


        public async Task<List<IncidenciaDto>> ObtenerHistorialIncidencia()
        {
            List<IncidenciaDto> incidencias = new List<IncidenciaDto>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("SEL_INCIDENCIAS", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var incidencia = new IncidenciaDto
                            {
                                id_incidencia = reader.GetInt32(0),
                                nombre_incidencia = reader.GetString(1),
                                descripcion = reader.GetString(2),
                                imagen = reader.GetString(3),
                                fecha_inicio = reader.GetString(4),
                                fecha_fin = reader.GetString(5),
                                id_usuario = reader.GetInt32(6),
                                id_estado = reader.GetInt32(7),
                                id_categoria = reader.GetInt32(8),
                                id_ubicacion = reader.GetInt32(9),
                            };
                            incidencias.Add(incidencia);
                        }
                    }
                }
            }
            return incidencias;
        }

        public static string ConvertFileToBase64(string filePath)
        {
            byte[] fileBytes = File.ReadAllBytes(filePath);
            return Convert.ToBase64String(fileBytes);
        }

    }
}
