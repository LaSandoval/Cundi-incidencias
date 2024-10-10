using Cundi_incidencias.Dto;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using Cundi_incidencias.Utility;
using Cundi_incidencias.Controllers;


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

            string insertQuery = @"INSERT INTO incidencia ( nombre_incidencia, descripcion, imagen, fecha_inicio, fecha_fin, id_usuario, id_estado, id_categoria, id_ubicacion)     
                           VALUES ( @nombre_incidencia, @descripcion, @imagen, @fecha_inicio, @fecha_fin, @id_usuario, @id_estado, @id_categoria, @id_ubicacion)";

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();
                using (SqlCommand programaCmd = new SqlCommand(insertQuery, con))
                {
                    using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@nombre_incidencia", incidencia.nombre_incidencia);
                        cmd.Parameters.AddWithValue("@descripcion", incidencia.descripcion);
                        cmd.Parameters.AddWithValue("@imagen", incidencia.imagen);
                        cmd.Parameters.AddWithValue("@fecha_inicio", incidencia.fecha_inicio);
                        cmd.Parameters.AddWithValue("@fecha_fin", incidencia.fecha_fin);
                        cmd.Parameters.AddWithValue("@id_usuario", incidencia.id_usuario);
                        cmd.Parameters.AddWithValue("@id_estado", incidencia.id_estado);
                        cmd.Parameters.AddWithValue("@id_categoria", incidencia.id_categoria);
                        cmd.Parameters.AddWithValue("@id_ubicacion", incidencia.id_ubicacion);

                        await cmd.ExecuteNonQueryAsync();
                    }

                    await con.CloseAsync();
                }

                return incidencia;
            }
        }
        public async Task<int>ActualizarIncidencia(string nombre_incidencia, string imagen, int id_categoria, int id_ubicacion)
        { 
        int filasactualizadas = 0;
        string query = @"UPDATE incidencia 
                     SET imagen = @imagen, 
                         id_categoria = @id_categoria, 
                         id_ubicacion = @id_ubicacion,
                     WHERE nombre_incidencia = @nombre_incidencia";
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@nombre_incidencia", nombre_incidencia);
                        cmd.Parameters.AddWithValue("@imagen", imagen);
                        cmd.Parameters.AddWithValue("@id_categoria", id_categoria);
                        cmd.Parameters.AddWithValue("@id_ubicacion", id_ubicacion);

                        filasactualizadas = await cmd.ExecuteNonQueryAsync();
}
                     await con.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar la incidencia: " + ex.Message);
            }

            return filasactualizadas;
        }

    }
}
