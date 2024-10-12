using Cundi_incidencias.Dto;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using Cundi_incidencias.Utility;
using Cundi_incidencias.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using static System.Net.Mime.MediaTypeNames;


namespace Cundi_incidencias.Repository
{
    public class IncidenciaRepository
    {
        private readonly string _connectionString;
        public IncidenciaRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IncidenciaDto> CrearIncidencia(IncidenciaDto incidencia)
        {

            string insertQuery = @"INSERT INTO incidencia ( nombre_incidencia, descripcion, imagen, fecha_inicio, fecha_fin, id_usuario, id_estado, id_categoria, id_ubicacion,fecha_creacion)     
                           VALUES ( @nombre_incidencia, @descripcion, @imagen, @fecha_inicio, @fecha_fin, @id_usuario, @id_estado, @id_categoria, @id_ubicacion, @fecha_creacion)";

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

                        await cmd.ExecuteNonQueryAsync();
                    }

                    await con.CloseAsync();
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
